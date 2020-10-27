
#include <stdlib.h>
#include <stdio.h>

struct le{
	int value;
	struct le * next;
};
typedef struct le listenelement;

typedef listenelement * list;
void insert(int v, list *l);
int delete_head(list *l);
void delete_all(list l);
int length(list l);
void print_list(list l);
int positionOf(int v, list l);
int delete_element(list *l, int number);
void filterEven(list *l);
void sort(int m, list *l);


int main(){
//create new List
list neu=NULL;
neu = malloc(sizeof(list));
neu -> next = NULL;
neu -> value = 0;

//insert Elements
insert(10,&neu);
insert(153,&neu);
insert(784,&neu);
insert(35,&neu);
insert(6,&neu);
insert(12,&neu);
insert(365,&neu);
print_list(neu);

printf("delete_head \n");
delete_head(&neu);
print_list(neu);

//Filter all odd elements
printf("FilterEven \n");
filterEven(&neu);
print_list(neu);

//check if '6' is in the List
printf("6 at %d \n", positionOf(6, neu));

//Sort list with insertionsort
sort(-1, &neu);
print_list(neu);

//check if '6' and '35' are in the list
printf("6 at %d \n", positionOf(6, neu));
printf("35 at %d \n", positionOf(35, neu));


return 0;}



void insert (int v, list *l){
	listenelement * new;
	new = malloc(sizeof(listenelement));
	new->value=v;
	new->next=*l;
	*l=new;
}

int delete_head(list *l){
	if (*l==NULL) return -1;
	list old =*l;
	*l=old->next;
	free(old);
	return 0;
}

void delete_all(list l){
	list next;
	while(l!=NULL){
		next=l->next;
		free(l);
		l=next;
	}
}

int length(list l){
	int count =0;
	while (l!=NULL){
		count++;
		l=l->next;
	}
	return count;
}

void print_list(list l){
	if (l==NULL) printf("leer\n");
	else{
		while (l ->next != NULL){
			printf("%d, ", l->value);
			l = l->next;
		}
		printf("\n");
	}
}

int positionOf(int v, list l){
	int count=0;
	while (l!= NULL){				//Loop durch l
		if(l->value==v){			//Abbruchkriterium wenn Wert gefunden
			break;
		}
		count++;
		l=l->next;
	}
	if (l!=NULL) return count;		//wenn l nicht bis zum Ende durchlaufen: Position zurückgeben
	else return -1;					//nicht in Liste enthalten
}

int delete_element(list *l, int number){
	if (*l==NULL) return -1;				//wenn Liste leer: Fehler
	if (number==0){							//wenn zu löschendes Element Head ist; Fkt-Aufruf von delete_head
		delete_head(l);
		return 0;
	}
	int count=0;							//Zählvariable
	list tmp = *l;							//temporäre Liste/Pointer auf Input-Liste
	listenelement *new;						//temporäres Listenelement
	new = malloc(sizeof(listenelement));	//Speicherplatzreservierung für temp. Listenelement
	while (tmp!=NULL){						//Loop durch temp. Liste von Pointer der Input-Liste
		if (count==number){					//wenn Position des Input-Elements(Index) erreicht
			new->next=tmp->next;			//temp. Listenelement aktuellen next Pointer zuweisen
			count=0;
			free(tmp);						//Speicherplatz freigeben
			break;
		}

		count++;
		tmp=tmp->next;
	}
	tmp=*l;									//Rücksetzen von tmp auf Pointer Input-Liste
	while(tmp!=NULL){						//Loop durch tmp
 		if (count==number-1){				//wenn Zählvariable ein Element vor Input-Element
			tmp->next=new->next;			//Zuweisung next-Pointer aus temp. Element
			break;
		}
		count++;
		tmp=(tmp)->next;
	}
	free(new);								//Speicherplatz für temp. Listenelement freigeben
	return 0;
}

void filterEven(list *l){
	int count=0;
	int val;							//temp. Var. um value zwischenzuspeichern und zu prüfen
	list tmp=*l;						//Pointer auf Input-Liste
	list last=*l;						//Pointer für zuletzt aufgerufenes Element
	while(tmp!=NULL){					//Loop durch tmp
		val=tmp->value;					//Zuweisung value von aktuellem Element auf val
		if (val%2==1){					//wenn val ungerade
			tmp=last->next;				//tmp wird Pointer auf next-Element von last
			delete_element(l, count);	//ungerades Element wird gelöscht
		}else{
			count++;					//weiterzählen
			tmp=tmp->next;
			last=tmp;					//neuer last-Pointer wird noch aktueller tmp-Pointer
		}
	}
}

void sort(int m, list *l){	
	list tmp=*l;										//tmp wird Pointer auf Input-Liste
	list i = NULL;									//Nullinitialisierung von i
	if(m<0){									//Entscheidung auf-/absteigend sortieren; m<0: absteigend; m>0: aufsteigend
		while(tmp != NULL) {					//Loop durch tmp
			list current = tmp;					//aktuelles Listenelemt = Pointer tmp
			tmp = tmp->next;					//tmp nächstes Element
			if(i == NULL || current->value > i->value) {	//wenn aktueller Wert größer als Wert von i (Head der neuen Liste) oder i=Null (leere neue Liste)
				current->next = i;				//aktuelles Element next-Pointer auf Head von i
				i = current;					//neuer Head-Pointer von i ist hinzugefügtes Element
			} else {							//wenn aktuelles Element kleiner als Wert von Head i
				list j = i;						//temp. Liste j init. mit Pointer i
				while(j != NULL) {				//Loop durch j
					if(j->next == NULL || current->value > j->next->value){	//wenn akt. Element größer als Wert vom nächsten j oder wenn nächstes j Null
						current->next = j->next;	//Pointer-Tausch j und akt. Element, d.h. Werte vertauschen
						j->next = current;			
						break;
					}
					j = j->next;
				}
			}
		}
	} else if(m>0){								//wie oben, aber aufsteigend (Änderungen kommentiert)
		while(tmp != NULL) {
			list current = tmp;
			tmp = tmp->next;
			if(i == NULL || current->value < i->value) {	//wenn aktueller Wert kleiner als Wert von i (Head der neuen Liste) oder i=Null (leere neue Liste)
				current->next = i;
				i = current;
			} else {
				list j = i;
				while(j != NULL) {
					if(j->next == NULL || current->value < j->next->value){	//wenn akt. Element kleiner als Wert vom nächsten j oder wenn nächstes j Null
						current->next = j->next;
						j->next = current;
						break;
					}
					j = j->next;
				}
			}
		}
	}
	*l=i;
}


