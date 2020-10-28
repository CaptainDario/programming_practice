/********************************
 * Author: Danny Puhan			*
 * Email puhan@uni-potsdam.de	*
 * Universtitaet Potsdam		*
 * Institut fuer Informatik		*
 ********************************/

#include "../include/kmeans.h"
#include "../include/debug.h"
#include "./kmeans_seq.c"

#define ERR 0
#define ITER_BREAK (iteration < max_iteration || max_iteration == 0)

/* standart K-means algorithm
after finishing result_out contains the number of centroid for each the data
and centroids contains the final centroids of the result*/
void kmeans(int dim, const double* data, int num_data, double* centroids, int num_centroids, int* result_out, int max_iteration) {

	double* distance = (double *) malloc(sizeof(double) * num_data * num_centroids);
	int* cluster_assignment = (int *) malloc(sizeof(int) * num_data);
	int* cluster_state_prev = (int *) malloc(sizeof(int) * num_data);
    Distance dist_fnc= EUKLID;
	BENCH_STRUCT(t_bench);
	double prev_total_distance;
	double total_distance;
	int iteration;

	if (!distance || !cluster_assignment || !cluster_state_prev) {
		exit(-1);
	}

	total_distance = DOUBLE_MAX;
	iteration = 0;

	printf("BEGIN ITERATION!\n");
/*** Calculation ***/
	do {
		TIME_GET(start_time);		

		// Cluster Konfiguration sichern
    	copy_array_int(num_data, cluster_assignment, cluster_state_prev);
		// Abstand zum Centroid des Clusters bestimmen
		calc_distances(dim, data, num_data, centroids, num_centroids,
				dist_fnc, distance);
		// Zuordnung zum Cluster
		assign_data_to_cluster( num_data, num_centroids, distance, cluster_assignment);
		prev_total_distance = total_distance;
		total_distance = sum_total_distance(num_data, distance, num_centroids, cluster_assignment);
		// Centroid des Clusters ermitteln
		calc_centroids(dim, num_data, num_centroids, data,
				cluster_assignment, centroids);
		iteration++;

		/*** benchmark time stuff ***/
		TIME_GET(end_time);
		t_bench.time_diff = TIME_DIFF(start_time,end_time);
		t_bench.time+=t_bench.time_diff;
		t_bench.time_iter+=t_bench.time_diff;
		if(t_bench.time_diff < t_bench.time_min) t_bench.time_min = t_bench.time_diff;
		if(t_bench.time_diff > t_bench.time_max) t_bench.time_max = t_bench.time_diff;
        /*** end bench ***/

	} while (prev_total_distance - total_distance  > ERR && ITER_BREAK );
	t_bench.time_iter = t_bench.time_iter/iteration;
	TIME_GET(start_time);

    /*** get the right result ***/
	if(total_distance - prev_total_distance != 0){
		calc_centroids(dim, num_data, num_centroids, data,
				cluster_state_prev, centroids);
		copy_array_int(num_data, cluster_state_prev, result_out);
	}else{
		copy_array_int(num_data, cluster_assignment, result_out);
	}
	TIME_GET(end_time);
	t_bench.time_diff = TIME_DIFF(start_time,end_time);
	t_bench.time+=t_bench.time_diff;

	/*>>>>>>>>>>>>>>>>> PRINTS <<<<<<<<<<<<<<<<<<<<*/
	printf("\nNUMBER OF ITERATIONS: %d!\n", iteration);
	printf("Average iteration time: %lf sec\n", t_bench.time_iter);
	printf("total run time: %lf sec\n\n", t_bench.time);
	printf("prev total distance:\n %.20lf LE\ntotal distance:\n %.20lf LE\n",prev_total_distance, total_distance);

	free(distance);
	free(cluster_assignment);
	free(cluster_state_prev);
}


