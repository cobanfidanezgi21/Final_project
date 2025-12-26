import cv2
import mediapipe as mp
import socket
import time

# --- AYARLAR ---
width, height = 1280, 720  # Kamera çözünürlüğü (Daha geniş alan için)

# Unity ile iletişim ayarları (UDP)
# Bu IP ve Port, Unity scriptindeki ile AYNI olmalı
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
serverAddressPort = ("127.0.0.1", 5052) 

# MediaPipe Kurulumu
mp_hands = mp.solutions.hands
hands = mp_hands.Hands(
    max_num_hands=1,             # Sadece 1 el takip etsin (Karışıklığı önler)
    min_detection_confidence=0.7,
    min_tracking_confidence=0.5
)
mp_draw = mp.solutions.drawing_utils

# Kamerayı Başlat
cap = cv2.VideoCapture(0)
cap.set(3, width)
cap.set(4, height)

print("Hazir! Kamera aciliyor...")

while True:
    success, img = cap.read()
    if not success:
        break

    # Görüntüyü çevir (Ayna etkisi için)
    img = cv2.flip(img, 1)
    
    # Renk formatını düzelt (BGR -> RGB)
    imgRGB = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)
    results = hands.process(imgRGB)

    data = [] # Gönderilecek veri paketi

    if results.multi_hand_landmarks:
        for hand_landmarks in results.multi_hand_landmarks:
            # Sadece işaret parmağının ucunu (Id: 8) alalım
            # İstersen avuç içi (Id: 9) veya bilek (Id: 0) de yapabiliriz
            x = hand_landmarks.landmark[8].x 
            y = hand_landmarks.landmark[8].y
            
            # Veriyi hazırla: "0.543,0.812" şeklinde string yap
            # Unity bunu alıp parçalayacak
            message = f"{x},{y}"
            
            # Unity'ye Fırlat! 🚀
            sock.sendto(str.encode(message), serverAddressPort)
            
            # Ekrana çizim yap (İskeletleri göster)
            mp_draw.draw_landmarks(img, hand_landmarks, mp_hands.HAND_CONNECTIONS)

            # Siyah ekrana da yazdıralım ki çalıştığını görelim
            print(f"Gonderiliyor: {message}")

    cv2.imshow("El Takibi - Unity Baglantisi", img)
    
    # 'q' tuşuna basınca çık
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

cap.release()
cv2.destroyAllWindows()