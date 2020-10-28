#!/bin/bash
#
#SBATCH --job-name=kmeans_on_cluster
#SBATCH --output=out_kmenas.out
#SBATCH --error=err_kmeans.err
#
#SBATCH --ntasks=96
#
#SBATCH --time=00:10:00

set -e

#Load modules
source /etc/profile.d/modules.sh
module purge
module load openmpi/4.0.0

#execute kmeans program
mpirun /home/klepoch/KPP_wettbewerb/kmeans/par/km_par 1000 10 0 10017 10017
