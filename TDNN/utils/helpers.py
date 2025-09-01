import torch
import jiwer
import os
import numpy as np
import glob
from torch.utils.data import Dataset,DataLoader
import inspect
import shutil


root = "c:/Projects/TinyReader/TDNN"

# dataset
class MFCCDataset(Dataset):
    def __init__(self, mfcc_files, phoneme_targets, phoneme2id):
        self.mfcc_files = mfcc_files
        self.phoneme_targets = phoneme_targets
        self.phoneme2id = phoneme2id

    def __len__(self):
        return len(self.mfcc_files)

    def __getitem__(self, idx):
        mfcc = np.load(self.mfcc_files[idx])  # shape: [n_mfcc, time]
        mfcc = mfcc.T  # [time, n_mfcc]
        phonemes = self.phoneme_targets[idx]
        target_ids = [self.phoneme2id[p] for p in phonemes if p in self.phoneme2id]
        return torch.tensor(mfcc, dtype=torch.float32), torch.tensor(target_ids, dtype=torch.long)

# make batches
def collate_fn(batch):
    mfccs, targets = zip(*batch)
    mfcc_lengths = torch.tensor([x.shape[0] for x in mfccs], dtype=torch.long)
    target_lengths = torch.tensor([len(t) for t in targets], dtype=torch.long)

    # Padding MFCCs (time dimension)
    mfccs_padded = torch.nn.utils.rnn.pad_sequence(mfccs, batch_first=True)  # [batch, max_time, n_mfcc]

    # Concatenate targets in 1D tensor for CTC
    targets_concat = torch.cat(targets)

    return mfccs_padded, targets_concat, mfcc_lengths, target_lengths

def check_cuda():
    if torch.cuda.is_available():
        device = torch.device("cuda")
        print(f"CUDA is available. Using GPU device: {torch.cuda.get_device_name(0)}")
    else:
        device = torch.device("cpu")
        print(f"CUDA isn't available. Using CPU device")
    return device

# dataloader for training and testing
def create_dataloader(mfcc_files, phoneme_targets, phoneme2id, batch_size=64,shuffle = False,num_work = 2):
    dataset = MFCCDataset(mfcc_files, phoneme_targets, phoneme2id)
    dataloader = DataLoader(dataset, batch_size=batch_size, shuffle=shuffle, collate_fn=collate_fn,num_workers=num_work,pin_memory=True)
    return dataloader

# decode for wer
def ctc_greedy_decode(log_probs, blank=0):
    preds = log_probs.argmax(dim=-1).transpose(0, 1)  # [batch, time]

    decoded_batch = []
    for pred in preds:
        prev = None
        decoded = []
        for p in pred.cpu().numpy():
            if p != blank and p != prev:
                decoded.append(p)
            prev = p
        decoded_batch.append(decoded)
    return decoded_batch

# measure model word error rate
def model_wer(model,device,dataloader,id2phoneme,show = False):
    model.eval()

    all_preds = []
    all_targets = []

    with torch.no_grad():
        for mfccs_padded, targets_concat, mfcc_lengths, target_lengths in dataloader:
            mfccs_padded = mfccs_padded.to(device)
            targets_concat = targets_concat.to(device)

            #!!!
            #mfccs_padded = mfccs_padded.transpose(1, 2)

            outputs = model(mfccs_padded)
            log_probs = outputs.transpose(0, 1)

            preds = ctc_greedy_decode(log_probs, blank=0)

            target_offset = 0
            for i in range(len(target_lengths)):
                length = target_lengths[i].item()
                target_seq = targets_concat[target_offset:target_offset+length].cpu().numpy().tolist()
                target_offset += length

                target_text = ' '.join([id2phoneme[id] for id in target_seq])
                pred_text = ' '.join([id2phoneme[id] for id in preds[i]])

                all_targets.append(target_text)
                all_preds.append(pred_text)

    if show:
        print("Real vs Predicted (first 5):")
        for i, (real, pred) in enumerate(zip(all_targets[:5], all_preds[:5]), 1):
            print(f"\n{i}. Real: {real}\n   Pred: {pred}")

    wer = jiwer.wer(all_targets, all_preds)
    return wer

# save models
class ModelSaver:
    def __init__(self):
        self.prev_file = ""

    def save_structure(self, model, name):
        folder = f"{root}/models/{name}"
        os.makedirs(folder, exist_ok=True)
        with open(f"{folder}/structure.txt", "w") as f:
            f.write(str(model))
        # with open(f"{folder}/source.txt", "w") as f:
        #     f.write(inspect.getsource(model.__class__))
    
    def log(self,name, value):
        dir = name.split("/")[0]
        os.makedirs(f"{root}/models/{dir}",exist_ok=True)
        with open(f"{root}/models/{name}", "a") as f:
            f.write(f"{value}\n")

    def clear_backup(self):
        files = glob.glob(os.path.join("backup", "*.pth"))
        for file in files:
            os.remove(file)

    def save_best_checkpoint(self, model,scheduler, optimizer, epoch, train_losses,val_losses,wers, name) -> None:
        prev_file = self.prev_file

        os.makedirs(f"{root}/models", exist_ok=True)
        new_file = f"{root}/models/{name}.pth"
        
        torch.save({
            'epoch': epoch,
            'model_state_dict': model.state_dict(),
            'optimizer_state_dict': optimizer.state_dict(),
            'scheduler_state_dict': scheduler.state_dict(),
            'train_losses': train_losses,
            'val_losses': val_losses,
            'wers': wers,
        }, new_file)

        if prev_file and os.path.exists(prev_file):
            os.remove(prev_file)

        self.prev_file = new_file

    def save_checkpoint(self,model,scheduler, optimizer, epoch, train_losses, val_losses, wers, path):
        torch.save({
            'epoch': epoch,
            'model_state_dict': model.state_dict(),
            'optimizer_state_dict': optimizer.state_dict(),
            'scheduler_state_dict': scheduler.state_dict(),
            'train_losses': train_losses,
            'val_losses': val_losses,
            'wers': wers,
        },f"{root}/{path}.pth")

    def load_checkpoint(self,model,scheduler, optimizer, path):
        checkpoint = torch.load(f"{root}/{path}.pth")
        model.load_state_dict(checkpoint['model_state_dict'])
        optimizer.load_state_dict(checkpoint['optimizer_state_dict'])
        scheduler.load_state_dict(checkpoint['scheduler_state_dict'])
        start_epoch = checkpoint['epoch']
        train_losses = checkpoint['train_losses']
        val_losses = checkpoint['val_losses']
        wers = checkpoint['wers']
        return model,optimizer,scheduler,start_epoch, train_losses, val_losses, wers
    
    def load_state(self,model,path):
        checkpoint = torch.load(f"{root}/{path}.pth")
        model.load_state_dict(checkpoint['model_state_dict'])
        return model