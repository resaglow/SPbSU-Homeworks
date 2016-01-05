#include <stdio.h>
#include <math.h>
#include <omp.h>
#include <Windows.h>

void fproc()
{
	double a;
	double b;
	double c;
	double d;
	double h;
	int k;
	long int n;
	double s;
	double s1;
	double value;
	{
		a = 0.0;
		b = 0.3141592653589793E1;
		c = 0.0;
		d = 0.3141592653589793E1;
		n = 5555555.0; // Достаточно большое число, чтобы была видна разница между многопоточным и однопотоным режимами
		h = (b - a) / n;
		s = 0.0;
#pragma omp parallel for reduction(+:s)
		for (k = 0; k <= n - 1; k++) {
			s += h*(pow(sin(k*h), 2.0) + pow(sin((1.0 + k)*h), 2.0)) / 2.0;
		}	
		s1 = 0.0;
#pragma omp parallel for reduction(+:s1)
		for (k = 0; k <= n - 1; k++) {
			s1 += h*(3.0*k*k*h*h + 3.0*pow(1.0 + k, 2.0)*h*h) / 2.0;
		}
		value = 0.1E1*s1*s;
		printf("%e\n", value);
		return;
	}
}


int main() {
	omp_set_dynamic(0);

	omp_set_num_threads(10);
	double start_time = GetTickCount();
	for (long i = 1; i < 2; i++) { fproc(); } // Оставлены циклы чтобы можно было менять кол-во тестов
	double end_time = GetTickCount();
	printf("Multi-threaded: %lf\n", (end_time - start_time));

	omp_set_num_threads(1);
	start_time = GetTickCount();
	for (long i = 1; i < 2; i++) { fproc(); }
	end_time = GetTickCount();
	printf("Single-threaded: %lf\n", (end_time - start_time));

	return 0;
}