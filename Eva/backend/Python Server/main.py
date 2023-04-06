import json
import vosk
import pyaudio
import struct
import math
import socket

model = vosk.Model("model/ru")  # Модель для распознавания русской речи
rec = vosk.KaldiRecognizer(model, 16000)

pa = pyaudio.PyAudio()

stream = pa.open(format=pyaudio.paInt16, channels=1, rate=16000, input=True, frames_per_buffer=8000)

final_result = ""


def rms(data):
    count = len(data) / 2
    format = "%dh" % count
    shorts = struct.unpack(format, data)
    sum_squares = 0.0
    for sample in shorts:
        n = sample * (1.0 / 32768)
        sum_squares += n * n
    return math.sqrt(sum_squares / count)


if __name__ == "__main__":
    # создание TCP сокета
    server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server_socket.bind(('localhost', 5555))
    server_socket.listen(1)

    print('Waiting for client connection...')
    client_socket, address = server_socket.accept()
    print(f'Connected to {address}')

    stream.start_stream()

    prew_res = ""
    while True:

        data = stream.read(2048, False)
        if len(data) == 0:
            break
        if rec.AcceptWaveform(data):
            res = json.loads(rec.Result())['text']
            if res == "": continue
            client_socket.send(("result:" + res).encode())
        else:
            res = json.loads(rec.PartialResult())['partial']
            if res == prew_res or res == "": continue
            print(res)
            prew_res = res
            client_socket.send(("partial:" + res).encode())
