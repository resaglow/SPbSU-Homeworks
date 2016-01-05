import cv2
import numpy as np

__author__ = 'resaglow'

img = np.zeros((500, 500, 3), np.uint8)
img.fill(255)
black_points = [[100, 100], [200, 100], [300, 400], [150, 300]]
for point in black_points:
    img[point[0], point[1]] = [0, 0, 0]

black_pixels = []

rows, cols = img.shape[:2]
for i in range(cols):
    for j in range(rows):
        if list(img[i, j]) == [0, 0, 0]:
            black_pixels.append((i, j))

print('Black pixels: {}'.format(black_pixels))

lines = []

for i in range(len(black_pixels)):
    for j in range(i + 1, len(black_pixels)):
        lines.append(
            np.cross(
                (black_pixels[i][0], black_pixels[i][1], 1),
                (black_pixels[j][0], black_pixels[j][1], 1)
            ))

intersections = []

for i in range(len(lines)):
    for j in range(i + 1, len(lines)):
        prod = np.cross(lines[i], lines[j])
        if prod[2] != 0:
            intersections.append(tuple(i / prod[2] for i in prod)[:2])

print('Intersections: {}'.format(intersections))

for i in range(len(intersections)):
    for j in range(i + 1, len(intersections)):
        if sorted(intersections[i]) != sorted(intersections[j]):
            cv2.line(img, intersections[i], intersections[j], (0, 0, 0))

for point in intersections:
    if 0 <= point[0] < cols and 0 <= point[1] < rows:
        cv2.circle(img, (point[0], point[1]), 3, (0, 0, 255), -1)

for point in black_points:
    cv2.circle(img, (point[0], point[1]), 2, (0, 0, 0), -1)

cv2.imshow('img', img)
cv2.waitKey(0)
cv2.destroyAllWindows()
