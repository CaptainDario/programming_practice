#/********************************
# * Author: Danny Puhan			*
# * Email puhan@uni-potsdam.de	*
# * Universtitaet Potsdam		*
# * Institut fuer Informatik		*
# ********************************/

BASE_CC=mpicc#gcc
COMMON_CFLAGS=-O2 
COMMON_LDFLAGS=-lcrypto -lrt
BASE_CFLAGS=-Wall -std=gnu99 -Wpedantic


TARGETS= km_par
LIB_TARGETS = lib_km_par\
	lib_kmeans_par

.PHONY: all
all:    km_lib\
        km_par\
        $(TARGETS)

all_vectorized:    	km_lib\
					km_par\
        			$(TARGETS)

par: km_par

km_par: km_par.c
	$(BASE_CC) $(COMMON_CFLAGS) $(BASE_CFLAGS) $^ $(COMMON_LDFLAGS) -o $@

km_lib: $(LIB_TARGETS)\
        lib_km_par\
        lib_kmeans_par

lib_km_par: lib_km_par.c 
	$(BASE_CC) $(COMMON_CFLAGS) $(BASE_CFLAGS) -shared -c -fPIC $^ $(COMMON_LDFLAGS) -o $@

km_lib_so: lib_km_par
	$(BASE_CC) $^ $(COMMON_LDFLAGS) -shared -o $^.so

lib_kmeans_par: kmeans_par.c 
	$(BASE_CC) $(COMMON_CFLAGS) $(BASE_CFLAGS) -c -fPIC $^ $(COMMON_LDFLAGS) -o $@

kmeans_lib_so: lib_kmeans_par
	$(BASE_CC) $^ $(COMMON_LDFLAGS) -shared -o $^.so
  
.PHONY: test
test:
	$ ./km_par 10000 7 0 4046397 3

.PHONY: clean
clean:
	rm -f *.o
	rm -f *.so
	rm -f $(TARGETS)
	rm -f $(LIB_TARGETS)

