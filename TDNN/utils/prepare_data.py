import numpy as np
import re
import glob
import os
import ast
import torchaudio
from torchaudio.transforms import MFCC, SlidingWindowCmn
import torchaudio.transforms as T
import torch
import librosa

proj_root = "c:/Projects/TinyReader/TDNN"

# create file with paths to audio and spoken text
# example of file
# /1089/134686/1089-134686-0001.flac STUFF IT INTO YOU HIS BELLY COUNSELLED HIM
# /1089/134686/1089-134686-0003.flac HELLO BERTIE ANY GOOD IN YOUR MIND
def create_audio_path_and_text(audio_folder,output_file):

    full_audio_path = os.path.join(proj_root, audio_folder)
    trans_files = glob.glob(os.path.join(full_audio_path, '**', '*.trans.txt'), recursive=True)
    output_file = os.path.join(proj_root,output_file)

    val = '\\'.join(re.split('/', output_file)[:-1])
    os.makedirs(val,exist_ok=True)
    
    transcripts = {}
    for tf in trans_files:
        with open(tf, 'r', encoding='utf-8') as f:
            for line in f:
                parts = line.strip().split(' ', 1)
                if len(parts) == 2:
                    transcripts[parts[0]] = parts[1]

    with open(output_file, 'w', encoding='utf-8') as f_out:
        for dirpath, _, filenames in os.walk(full_audio_path):
            for file in filenames:
                if file.lower().endswith('.flac'):
                    full_path = os.path.join(dirpath, file)

                    file_id = os.path.splitext(file)[0]
                    text = transcripts.get(file_id, '')

                    #for short audio's

                    # if len(text.split())>15 :
                    #     continue

                    f_out.write(f"{full_path} {text}\n")


# load maping list
def load_lexicon(lexicon_path):
    lexicon_path = os.path.join(proj_root,lexicon_path)
    lexicon = {}
    with open(lexicon_path, "r", encoding="utf-8") as f:
        for line in f:
            parts = line.strip().split()
            if len(parts) >= 2:
                word = parts[0].lower()
                phonemes = parts[1:]
                lexicon[word] = phonemes
    return lexicon

# map words into phonemes
def transcribe(text, lexicon, unk_token='SPN'):
    words = text.lower().strip().split()
    phonemes = []
    for word in words:
        if word in lexicon:
            phonemes.extend(lexicon[word])
        else:
            phonemes.append(unk_token)  # Unknown word
    return phonemes

# extracts mfcc and map words into phonemes
# example of file
# 1272-128104-0000.npy	['M', 'IH1', 'S', 'T', 'ER0', 'K', 'W', 'IH1']
def prepare_mfcc(audio_paths_file,output):

    audio_paths_file = os.path.join(proj_root,audio_paths_file)
    output = os.path.join(proj_root,output)

    paths = []
    transriptions = []
    with open(audio_paths_file, "r", encoding="utf-8") as f:
        lines = f.readlines()
        for line in lines:
            parts = line.strip().split(" ", 1)
            if len(parts) == 2:
                paths.append(parts[0])
                transriptions.append(parts[1])


    os.makedirs(output, exist_ok=True)
    lexicon_path = "utils/transcriptions.txt"

    lexicon = load_lexicon(lexicon_path)

    with open(f'{output}/trans.txt', "w", encoding="utf-8") as f:
        for i,path in enumerate(paths):
            name = re.split(r'[\\.]', path)[-2]

            y, sr = librosa.load(f'{path}', sr=16000)  
            #mfcc = librosa.feature.mfcc(y=y, sr=sr, n_mfcc=40,n_fft = 1024,hop_length=256) # 44.57 on dev на train 360 погано але можливо проблема в мережі
            #mfcc = librosa.feature.mfcc(y=y, sr=sr, n_mfcc=40,lifter=80) wer 49
            mfcc = librosa.feature.mfcc(y=y, sr=sr, n_mfcc=40)
            # cmvn
            mfcc = (mfcc - np.mean(mfcc, axis=1, keepdims=True)) / np.std(mfcc, axis=1, keepdims=True)

            np.save(f'{output}/{name}.npy', mfcc)
            phonemes = transcribe(transriptions[i],lexicon)
            f.write(f'{name}.npy\t{phonemes}\n')

# read all phonemes 
def read_phonemes(path):
    phonemes = []
    with open(path, 'r', encoding='utf8') as f:
        lines = f.readlines()
        for line in lines:
            phoneme = line.split(' ')[0]
            phonemes.append(phoneme)
    return phonemes

# map phonemes to id's
def phonemes_to_ids(phonemes):
    phoneme2id = {p: i for i, p in enumerate(phonemes)}
    id2phoneme = {i: p for p, i in phoneme2id.items()}
    return phoneme2id,id2phoneme

# make lists to upload into dataset
def prepare_input_data(path_to_mffc):
    mfcc_files = []
    phoneme_targets = []

    with open(f"{path_to_mffc}/trans.txt", "r", encoding="utf-8") as f:
        for line in f:
            parts = line.strip().split('\t')
            if len(parts) == 2:
                        mfcc_files.append(f"{path_to_mffc}/{parts[0]}")
                        phonemes = ast.literal_eval(parts[1])
                        phoneme_targets.append(phonemes)
    return mfcc_files,phoneme_targets


def create_mfcc_path_and_speaker_id(mfcc_folder,output_file):

    full_mfcc_path = os.path.join(proj_root, mfcc_folder)
    output_file = os.path.join(proj_root,output_file)

    val = '\\'.join(re.split('/', output_file)[:-1])
    os.makedirs(val,exist_ok=True)
    
    with open(output_file, 'w', encoding='utf-8') as f_out:
                for file in os.listdir(full_mfcc_path):
                    if file.lower().endswith('.npy'):
                        full_path = os.path.join(full_mfcc_path, file)
                        user_id = file.split("-")[0]
                        f_out.write(f"{full_path} {user_id}\n")

def prepare_xvector_input_data(path):
        mfcc_files = []
        user_ids = []

        full_path = os.path.join(proj_root,path)
        with open(f"{full_path}", "r", encoding="utf-8") as f:
            for line in f:
                parts = line.strip().split(" ")
                if len(parts) == 2:
                            mfcc_files.append(f"{parts[0]}")
                            user_ids.append(parts[1])
        return mfcc_files,user_ids
