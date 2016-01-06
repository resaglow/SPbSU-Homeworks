import math
import numpy as np


import os
import struct
from array import array


class MNIST(object):
    def __init__(self, path='.'):
        self.path = path

        self.test_img_fname = 't10k-images-idx3-ubyte'
        self.test_lbl_fname = 't10k-labels-idx1-ubyte'

        self.train_img_fname = 'train-images-idx3-ubyte'
        self.train_lbl_fname = 'train-labels-idx1-ubyte'

        self.test_images = []
        self.test_labels = []

        self.train_images = []
        self.train_labels = []

    def load_testing(self):
        ims, labels = self.load(os.path.join(self.path, self.test_img_fname),
                                os.path.join(self.path, self.test_lbl_fname))

        self.test_images = ims
        self.test_labels = labels

        return ims, labels

    def load_training(self):
        ims, labels = self.load(os.path.join(self.path, self.train_img_fname),
                                os.path.join(self.path, self.train_lbl_fname))

        self.train_images = ims
        self.train_labels = labels

        return ims, labels

    @classmethod
    def load(cls, path_img, path_lbl):
        with open(path_lbl, 'rb') as file:
            magic, size = struct.unpack(">II", file.read(8))
            if magic != 2049:
                raise ValueError('Magic number mismatch, expected 2049,'
                                 'got {}'.format(magic))

            labels = array("B", file.read())

        with open(path_img, 'rb') as file:
            magic, size, rows, cols = struct.unpack(">IIII", file.read(16))
            if magic != 2051:
                raise ValueError('Magic number mismatch, expected 2051,'
                                 'got {}'.format(magic))

            image_data = array("B", file.read())

        images = []
        for i in range(size):
            images.append([0] * rows * cols)

        for i in range(size):
            images[i][:] = image_data[i * rows * cols:(i + 1) * rows * cols]

        return images, labels


def gradient_descent(x, y, weights, num_steps, alpha):
    print('\nGradient descent start')

    def gradient_descent_step(cur_weights):
        gradient = np.zeros(param_len)
        for i in xrange(num_examples):
            nom = y[i] * x[i]
            denom = 1.0 + math.e ** (y[i] * np.dot(cur_weights, x[i]))
            gradient += nom / denom
        gradient /= -num_examples

        cur_weights -= alpha * gradient

        print('Step #{} done, cost = {}'.format(step, cost_function(x, y, cur_weights)))

        return cur_weights

    num_examples = len(y)

    for step in range(num_steps):
        weights = gradient_descent_step(weights)

    print('Gradient descent complete\n')

    return weights


def cost_function(x, y, weights):
    num_examples = len(x)
    cost = 0
    for i in xrange(num_examples):
        cost += math.log(1 + math.e ** (-y[i] * np.dot(weights, x[i])))
    cost /= num_examples
    return cost


def check(x, y, weights):
    print('Cost: {}'.format(cost_function(x, y, weights)))

    tp, tn, fp, fn = 0.0, 0.0, 0.0, 0.0

    for i in xrange(len(y)):
        s = np.dot(weights, x[i])
        h = 1.0 / (1 + math.e ** (-s))
        if h > 0.5 and y[i] == 1:
            tp += 1
            # print("Success")
        elif h <= 0.5 and y[i] == -1:
            tn += 1
            # print("Success")
        elif h > 0.5 and y[i] == -1:
            fp += 1
            # print("Failure")
        else:
            fn += 1
            # print("Failure")

    print('TP: {}, TN: {}, FP: {}, FN: {}'.format(tp, tn, fp, fn))
    print('Precision = {}, Recall = {}'.format(tp / (tp + fp), tp / (tp + fn)))


mndata = MNIST('./')
mndata.load_training()
mndata.load_testing()

# Learning parameters
target_number = 1  # currently detecting if a given number is 1
descent_steps_count = 20
learning_rate = 0.6

x_train = np.array(mndata.train_images, dtype=np.int64) / 255.0
y_train = np.array(map(lambda x: 1 if x == target_number else -1, mndata.train_labels), dtype=np.int64)

param_len = len(x_train[0])
initial_weights = np.zeros(param_len)

print("Cost of training data before training: {}".format(cost_function(x_train, y_train, initial_weights)))

new_weights = gradient_descent(x_train, y_train, initial_weights, descent_steps_count, learning_rate)

print("Training data after training:")
check(x_train, y_train, new_weights)


x_test = np.array(mndata.test_images, dtype=np.int64) / 255.0
y_test = np.array(map(lambda x: 1 if x == target_number else -1, mndata.test_labels), dtype=np.int64)

print("Test data after training:")
check(x_test, y_test, new_weights)