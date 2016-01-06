import math
import numpy as np
from mnist import MNIST


def gradient_descent(x, y, weights, num_steps, alpha, alpha_factor):
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

    initial_alpha = alpha
    for step in range(num_steps):
        weights = gradient_descent_step(weights)
        alpha += initial_alpha * alpha_factor

    print('Gradient descent complete\n')

    return weights


def cost_function(x, y, weights):
    num_examples = len(y)
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
        elif h <= 0.5 and y[i] == 1:
            fn += 1
            # print("Failure")

    print('TP: {}, TN: {}, FP: {}, FN: {}'.format(tp, tn, fp, fn))
    print('Precision = {}, Recall = {}'.format(tp / (tp + fp), tp / (tp + fn)))


def parse_mnist_binary(src_x, src_y):
    x = np.array(src_x, dtype=np.int64) / scaling_factor
    y = np.array(src_y, dtype=np.int64)
    zipped = zip(x, y)
    zipped = filter(lambda pair: pair[1] == target_number1 or pair[1] == target_number2, zipped)
    x, y = zip(*zipped)
    x, y = list(x), list(y)
    y = np.array(map(lambda elem: 1 if elem == target_number1 else -1, y), dtype=np.int64)
    return x, y


mndata = MNIST('./')
mndata.load_training()
mndata.load_testing()

# Learning parameters
target_number1 = 1
target_number2 = 0
descent_steps_count = 20
learning_rate = 2.5
scaling_factor = 255.0
alpha_increase_factor = 1 / 3.6

x_train, y_train = parse_mnist_binary(mndata.train_images, mndata.train_labels)

param_len = len(x_train[0])
initial_weights = np.zeros(param_len)

print("Cost of training data before training: {}".format(cost_function(x_train, y_train, initial_weights)))

new_weights = \
    gradient_descent(x_train, y_train, initial_weights, descent_steps_count, learning_rate, alpha_increase_factor)

print("Training data after training:")
check(x_train, y_train, new_weights)

x_test, y_test = parse_mnist_binary(mndata.test_images, mndata.test_labels)

print("Test data after training:")
check(x_test, y_test, new_weights)
