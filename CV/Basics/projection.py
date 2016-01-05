import math

__author__ = 'resaglow'

matrixX, matrixY = 600.0, 600.0
cX, cY = matrixX / 2.0, matrixY / 2.0

angleX, angleY = 60.0, 60.0

aX = -1.0
aY = 1.0
aZ = 6.0

f = (matrixX / 2.0 / math.tan(math.radians(angleX / 2.0)))

a_proj_x = f * aX / aZ + cX
a_proj_y = f * aY / aZ + cY

print(a_proj_x)
print(a_proj_y)
