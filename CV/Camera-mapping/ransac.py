import math
import random
import cv2
import numpy as np
import scipy.optimize as optimize

__author__ = 'resaglow'


def from_homogeneous(vector):
    z = vector[2]
    z = 0.0001 if z == 0 else z
    return [x / z for x in vector][:2]


def ransac():
    threshold = 90  # seems somewhat reasonable here (not a small image)
    best_sample = []
    best_sample_params = []
    best_sample_homography = []
    for _ in range(iter_count):
        cur_sample = random.sample(range(0, 16), 4)

        cur_homography = cv2.findHomography(img_points[cur_sample], chessboard_points[cur_sample])[0]
        converted_points = map(lambda x: from_homogeneous(np.dot(cur_homography, np.insert(x, 2, 1))), img_points)
        error = chessboard_points - converted_points

        filtered_points = filter(lambda (x, y): (x ** 2 + y ** 2) < threshold * threshold, error)

        if len(filtered_points) > len(best_sample):
            best_sample = np.copy(filtered_points)
            best_sample_params = np.copy(cur_sample)
            best_sample_homography = np.copy(cur_homography)

    print('Best sample params: {}'.format(best_sample_params))
    print('Best sample: \n{}\n'.format(best_sample))

    dst = cv2.perspectiveTransform(np.array([img_points]), best_sample_homography)
    proj_error = 0
    for i in range(8):
        proj_error += np.linalg.norm(dst[0][i] - chessboard_points[i])
    proj_error /= 8
    print('RANSAC error: {}\n'.format(proj_error))  # usually relatively small (<50) yet can be big

    return best_sample_params, best_sample_homography


def improve_precision():
    def points_diff(params):
        rotation_vector, shift = np.split(params, 2)
        rotation_matrix = cv2.Rodrigues(rotation_vector)[0]

        predicted_points = \
            map(lambda x: np.dot(k, np.dot(rotation_matrix, np.insert(x, 2, 1)) + shift),
                chessboard_points[best_params])
        predicted_points = map(lambda x: from_homogeneous(x), predicted_points)

        return (predicted_points - img_points[best_params]).flatten()

    focus = 6741
    k = np.float32([[focus, 0, img.shape[1] / 2],
                    [0, focus, img.shape[0] / 2],
                    [0, 0, 1]])
    k_inv = np.linalg.inv(k)

    h_n = np.dot(k_inv, homography)
    h_n /= np.linalg.norm(h_n[:, 0])

    r1 = h_n[:, 0]
    r2 = h_n[:, 1]
    t = h_n[:, 2]  # shifting vector
    r3 = np.cross(r1, r2)
    r = np.column_stack((r1, r2, r3))

    rotate_vector = cv2.Rodrigues(r)[0].flatten()
    transform_params = np.concatenate((rotate_vector, t))

    rotate_vector_found, shift_found = np.split(optimize.leastsq(points_diff, transform_params)[0], 2)
    rotation_matrix_found = cv2.Rodrigues(rotate_vector_found)[0]
    print('leastsq rotation matrix: \n{}'.format(rotation_matrix_found))
    print('leastsq shifting vector: \n{}\n'.format(shift_found))
    proj_error = 0
    for i in range(8):
        temp = np.dot(k, np.dot(rotation_matrix_found, np.insert(chessboard_points[i], 2, 0)) + shift_found)
        temp = from_homogeneous(temp)
        proj_error += np.linalg.norm(temp - img_points[i])
    proj_error /= 8
    print('RANSAC + nonlinear leastsq error: {}'.format(proj_error))


img = cv2.imread('chessboard.jpg', 0)

cell_size = 333

img_points = \
    np.float32([[536, 370], [228, 296], [425, 542], [1090, 835], [455, 681], [398, 402], [855, 451], [741, 616]])
chessboard_points = \
    cell_size * np.float32([[3, 2], [1, 1], [2, 3], [6, 6], [2, 4], [2, 2], [5, 3], [4, 4]])

prob_correct = 0.99
prob_single = 0.5  # prob to choose 'correct' point (0.5 since 8/16 are 'correct')
points_count = 4   # 4 points to build a homography

iter_count = int(
    math.log(1 - prob_correct) /
    math.log(1 - math.pow(prob_single, points_count))
)

for _ in range(8):
    img_points = np.vstack((
        img_points,
        np.float32([random.randint(1, img.shape[1]),
                    random.randint(1, img.shape[0])])
    ))
    chessboard_points = np.vstack((
        chessboard_points,
        np.float32([cell_size * random.randint(0, 6),
                    cell_size * random.randint(0, 6)])
    ))

best_params, homography = ransac()
improve_precision()
