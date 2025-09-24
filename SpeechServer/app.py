from fastapi import FastAPI
import torch
from model import TDNN 
import numpy as np
import pika
import io
import librosa
import threading

app = FastAPI()
model = None
phonemes = None
phoneme2id = None
id2phoneme = None

RABBIT_HOST = "rabbitmq"
QUEUE_NAME = "audios"

def start_consumer():
    connection = pika.BlockingConnection(pika.ConnectionParameters(host=RABBIT_HOST))
    channel = connection.channel()
    channel.queue_declare(queue=QUEUE_NAME, durable=False)  # збігається з існуючою

    def callback(ch, method, properties, body):
        print("Audio processing start...",flush=True)
        try:
            audio_bytes = io.BytesIO(body)
            y, sr = librosa.load(audio_bytes, sr=16000)
            mfcc = librosa.feature.mfcc(y=y, sr=sr, n_mfcc=40)
            mfcc = (mfcc - np.mean(mfcc, axis=1, keepdims=True)) / np.std(mfcc, axis=1, keepdims=True)

            with torch.no_grad():
                features = torch.from_numpy(mfcc).float().unsqueeze(0).transpose(1, 2)
                output = model(features)
                log_probs = output.transpose(0, 1)
                preds = ctc_greedy_decode(log_probs, blank=0)
                phoneme_preds = [[id2phoneme[int(p)] for p in seq] for seq in preds]
                result_text = " ".join(phoneme_preds[0])
                print("Recognized text:", result_text,flush=True)

            ch.basic_publish(
                exchange="",
                routing_key=properties.reply_to,
                properties=pika.BasicProperties(correlation_id=properties.correlation_id),
                body=result_text.encode("utf-8")
            )
            ch.basic_ack(delivery_tag=method.delivery_tag)
        except Exception as e:
            ch.basic_nack(delivery_tag=method.delivery_tag, requeue=False)
            print("Error processing audio:", e,flush=True)

    channel.basic_consume(queue=QUEUE_NAME, on_message_callback=callback, auto_ack=False)
    channel.start_consuming()

@app.on_event("startup")
def startup_event():
    global phonemes, phoneme2id, id2phoneme, model

    phonemes = read_phonemes("phonemes.txt")
    phoneme2id, id2phoneme = phonemes_to_ids(phonemes)
    model = TDNN(input_dim=40, output_dim=87)
    checkpoint = torch.load("model/model-state.pth", map_location="cpu")
    model.load_state_dict(checkpoint['model_state_dict'])
    model.eval()
    print("Model loaded",flush=True)
    threading.Thread(target=start_consumer, daemon=True).start()
    print("RabbitMq started",flush=True)

@app.get("/health")
async def health_check():
    return {"status": "ok"}

def read_phonemes(path):
    phonemes = []
    with open(path, 'r', encoding='utf8') as f:
        lines = f.readlines()
        for line in lines:
            phoneme = line.split(' ')[0]
            phonemes.append(phoneme)
    return phonemes

def phonemes_to_ids(phonemes):
    phoneme2id = {p: i for i, p in enumerate(phonemes)}
    id2phoneme = {i: p for p, i in phoneme2id.items()}
    return phoneme2id,id2phoneme

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