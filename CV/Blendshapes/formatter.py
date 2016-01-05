import numpy as np
import cv2


def rect_coords(rect, size, scale):
    # Add borders for smaller images
    # which don't fit due to 0.8 scale factor coeff
    def coord_with_border(num):
        if num < 0:
            return 0, abs(num)
        else:
            return num, 0

    rectx, recty, rectw, recth = rect
    new_height, new_width = size
    mid_x = int((rectx + rectw / 2) * scale)
    mid_y = int((recty + recth / 2) * scale)
    roi_x = mid_x - new_width / 2
    roi_y = mid_y - new_height / 2

    roi_x, border_x = coord_with_border(roi_x)
    roi_y, border_y = coord_with_border(roi_y)
    return roi_x, roi_y, border_x, border_y


def scale_amount(rect, size):
    scale_smooth_factor = 0.8

    new_height, new_width = size
    rect_h, rect_w = rect[2:]
    height_ratio = rect_h / new_height
    width_ratio = rect_w / new_width
    if height_ratio > width_ratio:
        new_recth = scale_smooth_factor * new_height
        scale = new_recth / rect_h
    else:
        new_rectw = scale_smooth_factor * new_width
        scale = new_rectw / rect_w
    return scale


def format_img(img, points, size):
    img = cv2.fastNlMeansDenoisingColored(img, None, 10, 10, 7, 21)

    new_height, new_width = size

    # Resize
    rect = cv2.boundingRect(np.array([points], np.int32))
    scale = scale_amount(rect, size)
    cur_height, cur_width = img.shape[:2]
    new_scaled_height = int(scale * cur_height)
    new_scaled_width = int(scale * cur_width)
    img = cv2.resize(img, (new_scaled_width, new_scaled_height))

    # Align rect to center
    cur_height, cur_width = img.shape[:2]
    roi_x, roi_y, border_x, border_y = rect_coords(rect, size, scale)
    roi_h = np.min([new_height - border_y, cur_height - roi_y])
    roi_w = np.min([new_width - border_x, cur_width - roi_x])

    # Crop
    crop = np.zeros((new_height, new_width, 3), img.dtype)
    crop[border_y:border_y + roi_h, border_x:border_x + roi_w] = (
        img[roi_y:roi_y + roi_h, roi_x:roi_x + roi_w]
    )

    # Scale and align points
    points[:, 0] = (points[:, 0] * scale) + (border_x - roi_x)
    points[:, 1] = (points[:, 1] * scale) + (border_y - roi_y)

    return crop, points
