import cv2
import numpy as np
import math

__author__ = 'resaglow'


def perspective(cur_point, angle):
    ax, ay, az, _ = cur_point
    f = (img_y / 2 * 1.0) / math.tan(math.radians(angle))
    return int(f * ((ax * 1.0) / az) + img_x / 2), int(f * ((ay * 1.0) / az) + img_y / 2)


def orthographic(points):
    orth_matrix = np.asmatrix(
        np.array([[int(img_x / 10), 0, 0, int(img_x / 5)],
                  [0, int(img_x / 10), 0, int(img_y / 5)],
                  [0, 0, 0, 0]]))
    return (orth_matrix * np.asmatrix(points).T).T


side = 6
z = math.sqrt(2) + math.sqrt(26)

square_points = np.array(
    [[0, 0, z, 1],
     [side, 0, z, 1],
     [0, side / math.sqrt(2), z + side / math.sqrt(2), 1],
     [side, side / math.sqrt(2), z + side / math.sqrt(2), 1]]
)

img_x = 550
img_y = 400
img = np.zeros((img_y, img_x, 3), np.uint8)
img.fill(255)

projections = []

for point in square_points:
    projections.append(perspective(point, 45))

cv2.line(img, projections[0], projections[1], [0, 0, 0])
cv2.line(img, projections[1], projections[3], [0, 0, 0])
cv2.line(img, projections[3], projections[2], [0, 0, 0])
cv2.line(img, projections[2], projections[0], [0, 0, 0])

cv2.imshow('Perspective', img)

img.fill(255)
projection_mat = np.asarray(orthographic(square_points))
projections = [(int(point[0]), int(point[1])) for point in projection_mat]

cv2.line(img, projections[0], projections[1], [0, 0, 0])
cv2.line(img, projections[1], projections[3], [0, 0, 0])
cv2.line(img, projections[3], projections[2], [0, 0, 0])
cv2.line(img, projections[2], projections[0], [0, 0, 0])

cv2.imshow('Orthographic', img)

cv2.waitKey(0)
cv2.destroyAllWindows()
