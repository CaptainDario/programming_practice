#This script is only being used to validate the parallel algorithm.

import subprocess
import os
import sys
import random
import re

tot_dist_re = "\\ntotal distance:\\n.\d+\.\d+.LE"

def generate_random_kmeans_params():
    max_iteration = random.randint(3, 100)
    num_centroids = random.randint(2, 50)
    generate_mode = 0
    size          = random.randint(max_iteration+1, 20000)
    dim           = random.randint(100, 20000)

    return [str(i) for i in [max_iteration, num_centroids, generate_mode, size, dim]]


if __name__ == "__main__":

    args, run_sample = sys.argv, False

    #check arguments
    if(len(args) <= 1):
        print("No number of tests specified! Running the sample...")
        tests = 1
        run_sample = True
    else:
        if(args[1] == "sample"):
            tests = 1
            run_sample = True
        else:
            tests = int(sys.argv[1])
    if(run_sample):
        print("Will run sample.")
    else:
        print("Will run", tests, "tests")
    
    #run tests
    correct, wrong = 0, 0
    error_runs     = []
    par, seq       = "", ""
    for i in range(0, tests):
        os.chdir("/mnt/d/uni/6semester/KPP/MPI-kmeans/kmeans/")

        #generate parameters
        params = None
        if(not run_sample):
            params = generate_random_kmeans_params()
        else:
            params = ["1000", "10", "0", "117", "100"]

        print("Running parallel...:")
        par = subprocess.run(["mpirun", "-np", "7", "./par/km_par", *params], capture_output=True)
        #print(par.stdout)

        print("Running sequential...:")
        seq = subprocess.run(["./seq/km_seq", *params], capture_output=True)
        #print(seq.stdout)

        print("Comparing...:")
        seq_out_ret = re.findall(tot_dist_re, str(seq.stdout, "utf-8"))
        par_out_ret = re.findall(tot_dist_re, str(par.stdout, "utf-8"))
        if(seq_out_ret == par_out_ret):
            correct += 1
            print(seq_out_ret, "\n", par_out_ret)
        else:
            wrong += 1
            error_runs.append(params)
        
        print("")


    #output wrong/correct tests
    print("Correct:", correct, "Wrong:", wrong)
    if(len(error_runs) > 0):
        print("Erros appeared in: ")
        for err in error_runs:
            print(err)

