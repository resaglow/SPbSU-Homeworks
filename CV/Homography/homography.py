import cv2
import numpy as np
import math

__author__ = 'resaglow'

img1 = cv2.imread('hall405.jpg')

m1 = np.float32(((505, 129), (630, 74), (506, 308), (631, 309)))
n = 500
m2 = np.float32(((0, 0), (n, 0), (0, n), (n, n)))

H = cv2.findHomography(m1, m2)[0]
res = cv2.warpPerspective(img1, H, (n, n))

sampleSources = np.float32(((583, 238, 1), (583, 281, 1), (565, 243, 1), (566, 282, 1)))
sampleExpected = np.float32(((340, 334, 1), (341, 435, 1), (272, 339, 1), (275, 435, 1)))

sampleGot = [(np.dot(H, x) / np.dot(H, x)[2]) for x in sampleSources]

norms = []
for i in range(4):
    y1, y2 = sampleGot[i][0] - sampleExpected[i][0], sampleGot[i][1] - sampleExpected[i][1]
    norms.append(math.sqrt(y1 * y1 + y2 * y2))
norm_average = sum(norms) / 4.0

print(norms)
print(norm_average)

cv2.imshow('result', res)
cv2.waitKey(0)
cv2.destroyAllWindows()
