import cv2
import numpy as np

__author__ = 'resaglow'


def transform_remap(src_tl, src_br, dst_tl, dst_br):
    rows, cols = img.shape[:2]

    zoom_x = (src_br[0] - src_tl[0]) / (dst_br[0] - dst_tl[0])
    zoom_y = (src_br[1] - src_tl[1]) / (dst_br[1] - dst_tl [1])

    map_x = np.zeros((rows, cols), np.float32)
    map_y = np.zeros((rows, cols), np.float32)

    for j in range(rows):
        for i in range(cols):
            map_x[j, i] = zoom_x * (i - dst_tl[0]) + src_tl[0]
            map_y[j, i] = zoom_y * (j - dst_tl[1]) + src_tl[1]

    return cv2.remap(img, map_x, map_y, cv2.INTER_LINEAR)


img = cv2.imread('source.jpg')

height, width = img.shape[:2]

cv2.imshow('old', img)
img = transform_remap((0, 0), (20, 20), (10, 0), (20, 10))
cv2.imshow('new', img)
cv2.waitKey(0)
cv2.destroyAllWindows()
