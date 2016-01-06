import math
import numpy as np


def normalize(v):
    x, y, z = v[0], v[1], v[2]
    norm_factor = math.sqrt(x * x + y * y + z * z)
    return v / norm_factor


v1 = np.array([1, 1, -2])
v2 = np.array([1, 2, -3])

v1 = normalize(v1)
v2 = normalize(v2)
v3 = normalize(np.cross(v1, v2))

points_original = []
for i in range(100):
    points_original.append(np.random.uniform() * v1 + np.random.uniform() * v2)
points_original = np.array(points_original)

points_slided = points_original + np.random.normal(0, 0.1, [len(points_original), 3])


def leastsq_precision(points):
    points_xs, points_ys, points_zs = points[:, 0], points[:, 1], points[:, 2]

    x = np.column_stack((points_xs, points_ys))
    xt = x.T
    params = np.linalg.inv(xt.dot(x)).dot(xt).dot(points_zs)

    a, b, c = params[0], params[1], 1  # 1 for this specific plane
    return np.linalg.norm(abs(normalize(np.array([a, b, c]))) - abs(v3))


log_file = open('linear_leastsq.log', 'w')
for i in range(10, 110, 10):
    log_file.write('{}\n'.format(leastsq_precision(points_slided[:i])))
