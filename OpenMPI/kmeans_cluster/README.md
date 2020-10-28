# MPI-kmeans
A parallel MPI implementation of the kmeans algorithm.
This is a very poor implementation which does not scale well on a cluster.
It was mainly a first test of MPI for me without further optimization.


Requirements:
    * command line arguments:
      * max iterations
      * number of centroids
      * filename to read from (option 1)
      * generate data (Option 0)
      * number of data vectors
      * dimension of the data vectors
    
    *  option 0 generates data at runtime
    *  one mpi process per core (and node)

This program was made during the course "Konzepte Paralleler Programmierung".