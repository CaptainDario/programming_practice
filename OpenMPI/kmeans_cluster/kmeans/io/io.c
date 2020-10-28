/********************************
 * Author: Danny Puhan			*
 * Email puhan@uni-potsdam.de	*
 * Universtitaet Potsdam		*
 * Institut fuer Informatik		*
 ********************************/
#include <stdio.h>
#include <string.h>
#include <stdint.h>
#include <stdlib.h>
#include "../include/common.h"

bool file_exist(char* filename){
    FILE *fp;
    if((fp = fopen(filename, "r"))){    
        fclose(fp);
        return true;
    }
    return false;
}

void read_size_dim(char* filename,int* s, int* d){
FILE *fp;
        fp = fopen(filename, "r");
    if(fp!=NULL){ 
       if( fscanf(fp, "%d;", s))
           if( fscanf(fp, "%d;", d) );            
    }  
    fclose(fp);
}

void read_kmeans_data(char* filename,int size, int dim, double* val){
    FILE *fp;
    
    fp = fopen(filename, "r");
    int buffer = 0;
    int i = 0;
    char *frmt = "%d,";
    
    if(fp!=NULL){ 
        if( fscanf(fp, "%d;", &size))
            if( fscanf(fp, "%d;", &dim) );            
        if(size==0 && dim==0){return;}
        
    }else{return;}       
       //double* val = (double *) malloc(sizeof(double) * size * dim);
    while ( fp != NULL && (fscanf(fp, frmt, &buffer)) > 0){            
        val[i] = buffer;
        i++;
        if(i==(size)*(dim)){frmt="%d;";}
    }
    fclose(fp);
}
