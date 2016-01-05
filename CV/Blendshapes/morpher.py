import numpy as np
import scipy.spatial as spatial
import cv2


def weighted_average_points(start_points, end_points, percent):
    if percent <= 0:
        return end_points
    elif percent >= 1:
        return start_points
    else:
        return np.asarray(start_points * percent + end_points * (1 - percent), np.int32)


def weighted_average_img(img1, img2, percent):
    if percent <= 0:
        return img2
    elif percent >= 1:
        return img1
    else:
        return cv2.addWeighted(img1, percent, img2, 1 - percent, 0)


def bilinear_interpolate(img, coords):
    int_coords = np.int32(coords)
    x0, y0 = int_coords
    dx, dy = coords - int_coords

    q11 = img[y0, x0]
    q21 = img[y0, x0 + 1]
    q12 = img[y0 + 1, x0]
    q22 = img[y0 + 1, x0 + 1]

    btm = q21.T * dx + q11.T * (1 - dx)
    top = q22.T * dx + q12.T * (1 - dx)
    inter_pixel = top * dy + btm * (1 - dy)

    return inter_pixel.T


def grid_coordinates(points):
    xmin = np.min(points[:, 0])
    xmax = np.max(points[:, 0]) + 1
    ymin = np.min(points[:, 1])
    ymax = np.max(points[:, 1]) + 1
    return np.asarray(
        [(x, y) for y in range(ymin, ymax) for x in range(xmin, xmax)],
        np.uint32
    )


def affine_transforms(vertices, src_points, dest_points):
    ones = [1, 1, 1]
    for tri_indices in vertices:
        src_tri = np.vstack((src_points[tri_indices, :].T, ones))
        dst_tri = np.vstack((dest_points[tri_indices, :].T, ones))
        mat = np.dot(src_tri, np.linalg.inv(dst_tri))[:2, :]
        yield mat


def deform_image(src_img, dest_img, src_points, dest_points, dest_shape, percent, dtype=np.uint8):
    num_chans = 3
    src_img = src_img[:, :, :3]

    rows, cols = dest_shape[:2]
    result_img = np.zeros((rows, cols, num_chans), dtype)

    delaunay = spatial.Delaunay(dest_points)
    tri_affines = np.asarray(list(affine_transforms(
        delaunay.simplices, src_points, dest_points)))

    roi_coords = grid_coordinates(dest_points)
    result_img[:, :] = (
        percent * dest_img[:, :] + (1 - percent) * src_img[:, :] if percent < 0.5 else
        percent * src_img[:, :] + (1 - percent) * dest_img[:, :]
    )
    roi_tri_indices = delaunay.find_simplex(roi_coords)

    for simplex_index in xrange(len(delaunay.simplices)):
        coords = roi_coords[roi_tri_indices == simplex_index]
        num_coords = len(coords)
        out_coords = np.dot(tri_affines[simplex_index],
                            np.vstack((coords.T, np.ones(num_coords))))
        x, y = coords.T
        result_img[y, x] = bilinear_interpolate(src_img, out_coords)

    return result_img
