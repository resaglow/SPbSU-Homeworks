import argparse
import scipy.ndimage
import numpy as np
import cv2
import os
import subprocess
import formatter
import morpher


class Animation(object):

    def __init__(self, filename, fps, w, h):
        self.filename = filename
        self.counter = 0

        if filename is None:
            self.video = None
        else:
            fourcc = cv2.cv.FOURCC(*'mp4v')
            self.video = cv2.VideoWriter(filename, fourcc, fps, (w, h), True)

    def write(self, img, num_times=1):
        frame = np.copy(img)
        if img.shape[2] == 3:
            frame[..., 0], frame[..., 2] = img[..., 2], img[..., 0]

        for i in xrange(num_times):
            self.video.write(frame)
            self.counter += 1

    def end(self):
        print(self.filename + ' saved')
        self.video.release()


def get_features(imgpath):
    directory = os.path.dirname(os.path.realpath(__file__))
    stasm_path = os.path.join(directory, 'bin/stasm')
    data_folder = os.path.join(directory, 'data')
    command = '"{0}" -f "{1}" "{2}"'.format(stasm_path, data_folder, imgpath)
    s = subprocess.check_output(command, shell=True)
    if s.startswith('No face found'):
        return []
    else:
        points = np.array(
            [pair.split(' ') for pair in s.rstrip().split('\n')],
            np.int32
        )
        return points


def init_points(path, size):
    if not os.path.isfile(path):
        print('File path is not correct, exiting')
        exit(1)

    img = scipy.ndimage.imread(path)[..., :3]
    points = get_features(path)

    if len(points) == 0:
        print('No face in %s'.format(path))
        return None, None
    else:
        return formatter.format_img(img, points, size)


def morph(src_file, dest_file, out_file):
    if out_file is None:
        print('No out_video param supplied, exiting')
        exit(1)

    width, height = 400, 500
    num_frames, fps = 20, 10

    animation = Animation(out_file, fps, width, height)
    size = (height, width)
    images_points_gen = [init_points(src_file, size), init_points(dest_file, size)]

    src_img, src_points = images_points_gen[0]
    dest_img, dest_points = images_points_gen[1]

    size = (height, width)
    border_frames = np.clip(int(fps * 0.15), 1, fps)  # show border images longer
    num_frames -= (border_frames * 2)  # no need to process them

    animation.write(src_img, border_frames)

    for percent in np.linspace(1, 0, num=num_frames):
        points = morpher.weighted_average_points(src_points, dest_points, percent)
        src_face = morpher.deform_image(src_img, dest_img, src_points, points, size, percent)
        end_face = morpher.deform_image(dest_img, src_img, dest_points, points, size, percent)
        average_face = morpher.weighted_average_img(src_face, end_face, percent)
        animation.write(average_face)

    animation.write(dest_img, border_frames)

    animation.end()


if __name__ == "__main__":
    parser = argparse.ArgumentParser(description='Blendshapes')

    parser.add_argument('--src')
    parser.add_argument('--dest')
    parser.add_argument('--out_file')

    args = parser.parse_args()

    morph(args.src, args.dest, args.out_file)
