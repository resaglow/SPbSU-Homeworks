import cv2
import numpy as np
import scipy.optimize as optimize

__author__ = 'resaglow'


def from_homogeneous(vector):
    return [x / vector[2] for x in vector][:2]


def points_diff(params):
    rotation_vector, shift = np.split(params, 2)
    rotation_matrix = cv2.Rodrigues(rotation_vector)[0]

    predicted_points = \
        map(lambda x: np.dot(k, np.dot(rotation_matrix, np.insert(x, 2, 0)) + shift),
            chessboard_points[0:points_used_count])
    predicted_points = map(lambda x: from_homogeneous(x), predicted_points)

    return (predicted_points - img_points[0:points_used_count]).flatten()


img = cv2.imread('chessboard.jpg', 0)

cell_size = 333

img_points = \
    np.float32([[536, 370], [228, 296], [425, 542], [1090, 835], [455, 680], [398, 402], [855, 451], [741, 616]])
chessboard_points = \
    cell_size * np.float32([[3, 2], [1, 1], [2, 3], [6, 6], [2, 4], [2, 2], [5, 3], [4, 4]])

h = cv2.findHomography(chessboard_points[0:4], img_points[0:4])[0]

focus = 6741
k = np.float32([[focus, 0, img.shape[1] / 2],
                [0, focus, img.shape[0] / 2],
                [0, 0, 1]])
k_inv = np.linalg.inv(k)

h_n = np.dot(k_inv, h)
h_n /= np.linalg.norm(h_n[:, 0])

r1 = h_n[:, 0]
r2 = h_n[:, 1]
t = h_n[:, 2]  # shifting vector
r3 = np.cross(r1, r2)
r = np.column_stack((r1, r2, r3))

rotate_vector = cv2.Rodrigues(r)[0].flatten()
transform_params = np.concatenate((rotate_vector, t))

logFile = open('nonlinear_leastsq.log', 'w')
for points_used_count in range(4, 9):
    params_found = optimize.leastsq(points_diff, transform_params)[0]
    rotation_vector_found, shift_found = np.split(params_found, 2)
    rotation_matrix_found = cv2.Rodrigues(rotation_vector_found)[0]

    projError = 0
    for i in range(8):
        temp = np.dot(k, np.dot(rotation_matrix_found, np.insert(chessboard_points[i], 2, 0)) + shift_found)
        temp = from_homogeneous(temp)
        projError += np.linalg.norm(temp - img_points[i])
    projError /= 8

    print('{}'.format(projError))
    logFile.write('{}\n'.format(projError))
logFile.close()
