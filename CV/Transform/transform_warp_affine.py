import cv2
import numpy as np

__author__ = 'resaglow'


def transform_affine(src_tl, src_br, dst_tl, dst_br):
    scale_x = float(dst_br[0] - dst_tl[0]) / (src_br[0] - src_tl[0])
    scale_y = float(dst_br[1] - dst_tl[1]) / (src_br[1] - src_tl[1])

    trans_x = dst_tl[0] - scale_x * src_tl[0]
    trans_y = dst_tl[1] - scale_y * src_tl[1]

    m = np.float32([[scale_x, 0, trans_x], [0, scale_y, trans_y]])

    rows, cols = img.shape[:2]
    return cv2.warpAffine(img, m, (cols, rows))


img = cv2.imread('source.jpg')

height, width = img.shape[:2]

cv2.imshow('old', img)
img = transform_affine((0, 0), (20, 20), (10, 0), (20, 10))
cv2.imshow('new', img)
cv2.waitKey(0)
cv2.destroyAllWindows()
