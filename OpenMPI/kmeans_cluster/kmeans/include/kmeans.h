/********************************
 * Author: Danny Puhan			*
 * Email puhan@uni-potsdam.de	*
 * Universtitaet Potsdam		*
 * Institut fuer Informatik		*
 ********************************/
#ifndef KMEANS_H_INCLUDED
#define KMEANS_H_INCLUDED

#include <stdbool.h>
#include <stdio.h>
#include <math.h>
#include <stdlib.h>
#include <stddef.h>
#include <stdint.h>
#include <time.h>
#include <float.h>
#include <limits.h>
#include <assert.h>
#include <ctype.h>

typedef int Distance;
#define EUKLID 0
#define MANHATTEN 1
#define GENERATE 0
#define FILE_INPUT 1
#define MIN_ARGS 5
#define MAX_ITER_ARG 1
#define K_ARG 2
#define MODE_ARG 3
#define FILE_ARG 4
#define SIZE_ARG 4
#define DIM_ARG 5

#define DOUBLE_MAX ((double)1.79769313486231570815e+308L) //DBL_MAX

void kmeans(int dim, const double *data_in, int num_data, double *init_clusters_in,
		int num_cluster, int *result_out, int max_iteration);
double euklid_distance(int dim, const double* dataPoint, double* centerPoint);
double manhatten_distance(int dim, const double* dataPoint, double* centerPoint);

void calc_data_cluster_distance(int dim, const double* dataPoint, double* centerPoints,
		int num_center, Distance dist_descriptor, double* distance);
void calc_distances(int dim, const double* data_points, int num_data,
		double* center_points, int num_center,
		Distance dist_descriptor, double* distance);
double sum_total_distance(int num_data, double *distances, int num_center, int *cluster_assignment);
void assign_data_to_cluster(int num_data, int num_center,
		double *distances, int *cluster_assignment);

//*
void calc_centroids(int dim, int num_data, int num_center, const double *data,
		int *cluster_assignment, double *clusters);
double calc_total_distance(int dim, int num_data, double *data,
		double *centerPoints, int *cluster_assignment,
		double (*distance_function)(int, double[], double[]));
//*/
void copy_array(void* src, void* dst, int len);
void copy_array_dbl(int num_data, double *src, double *dst);
void copy_array_int(int num_data, int *src, int *dst);
bool is_equal_array_int(int num_data, int *a, int *b);
void print_array_int(int num_data, int *array);
/*** TIME ***/
#define TIME_GET(timer) \
	struct timespec timer; \
	clock_gettime(CLOCK_MONOTONIC, &timer)

#define TIME_DIFF(timer1, timer2) \
	((timer2.tv_sec * 1.0E+9 + timer2.tv_nsec) - \
	 (timer1.tv_sec * 1.0E+9 + timer1.tv_nsec)) / 1.0E+9

/*** STRUCTS ***/
typedef struct time_info t_info;
struct time_info{
		double time_diff;
		double time;
		double time_min;
		double time_max;
		double time_iter;
};

#define BENCH_STRUCT(time_struct)		\
	struct time_info time_struct;			\
	time_struct.time_diff = 0;			\
	time_struct.time = 0;				\
	time_struct.time_min = DOUBLE_MAX;	\
	time_struct.time_max = 0;			\
	time_struct.time_iter = 0		

#endif
