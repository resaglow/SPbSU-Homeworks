import cv2
import numpy as np

__author__ = 'resaglow'

imgFile = cv2.imread('low-contrast.png', cv2.CV_LOAD_IMAGE_GRAYSCALE)
hist = cv2.calcHist([imgFile], [0], None, [256], [0, 256])

low, high = int(imgFile.min()), 255

width = imgFile.shape[1]
threshold = width * (10.0 / 750)    # heuristic from the given image
for i in reversed(range(256 - 1)):  # there are often some 255 pixels, but aren't 254, 253 etc.
    if hist[i][0] >= threshold:
        high = i
        break

newImg = np.array(map(lambda x: map(lambda y: min(255 * (y - low) / (high - low), 255), x),
                      imgFile.tolist()),
                  dtype=np.uint8)

cv2.imshow('result', newImg)
cv2.waitKey(0)
cv2.destroyAllWindows()
