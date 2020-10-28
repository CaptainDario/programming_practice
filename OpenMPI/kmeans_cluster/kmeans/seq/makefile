#/********************************
# * Author: Danny Puhan			*
# * Email puhan@uni-potsdam.de	*
# * Universtitaet Potsdam		*
# * Institut fuer Informatik		*
# ********************************/

BASE_CC=gcc
COMMON_CFLAGS=-O2 -pg
COMMON_LDFLAGS=-lcrypto -lrt
BASE_CFLAGS=-Wall -std=gnu99 -Wpedantic


TARGETS= km_seq
LIB_TARGETS = lib_km_seq\
	lib_kmeans_seq

.PHONY: all
all:    km_lib\
        km_seq\
        $(TARGETS)

seq: km_seq

km_seq: km_seq.c
	$(BASE_CC) $(COMMON_CFLAGS) $(BASE_CFLAGS) $^ $(COMMON_LDFLAGS) -o $@

km_lib: $(LIB_TARGETS)\
        lib_km_seq\
        lib_kmeans_seq

lib_km_seq: lib_km_seq.c 
	$(BASE_CC) $(COMMON_CFLAGS) $(BASE_CFLAGS) -shared -c -fPIC $^ $(COMMON_LDFLAGS) -o $@

km_lib_so: lib_km_seq
	$(BASE_CC) $^ $(COMMON_LDFLAGS) -shared -o $^.so

lib_kmeans_seq: kmeans_seq.c 
	$(BASE_CC) $(COMMON_CFLAGS) $(BASE_CFLAGS) -c -fPIC $^ $(COMMON_LDFLAGS) -o $@

kmeans_lib_so: lib_kmeans_seq
	$(BASE_CC) $^ $(COMMON_LDFLAGS) -shared -o $^.so
  
.PHONY: test
test:
	$ ./km_seq 10000 7 0 4046397 3

.PHONY: clean
clean:
	rm -f *.o
	rm -f *.so
	rm -f $(TARGETS)
	rm -f $(LIB_TARGETS)

