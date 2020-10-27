import random
import math


trenner = "---------------------------------------------------------------------------------------------------------------------"

#############################################
### MERGESORT ###############################
#############################################
def mergeSort(L, ascending = True):
	print(trenner)
	print('Mergesort, Parameter L:')
	print(L)
	if(ascending):
		print("SORTIERE AUFSTEIGEND...")
	else:
		print("SORTIERE ABSTEIGEND...")
	# hier sollte Ihre Implementierung des Mergesort-Verfahrens stehen.
	
	#zerteilen der Liste
	print("\n" + "ZERTEILEN DER LISTE")
	print(trenner)
	#die anfangsliste in zwei Teile zerteilen
	length = len(L)
	L.extend(Zerteile(L))
	L[:] = L[length:]
	print()

	#diese beiden Teile zerteilen bis nur noch Einzelelemente(Basisfall) ueber sind
	n = 0
	while(len(L) < length):
		#falls das n-te Element noch nicht der Basisfall ist
		if(len(L[n]) > 1):
			#das Element zerteilen und an das Ende anfuegen
			L.extend(Zerteile(L.pop(n)))	
			print()
		#falls das n-te Element schon der Basisfall ist 
		else:
			#das nachfolgende Element betrachten
			n += 1

	print(L)
	#ordnen und zusammenfuegen
	print("\n" + "ORDNEN UND ZUSAMMENFUEGEN")
	print(trenner)

	#solange mehr als ein Element zu sortieren ist...
	while(True):
		#wenn es nur noch zwei Elemente zu sortieren gibt
		if(len(L) == 2):
			#fuege die beiden sortiert zusammen
			L.extend(Vereine(L.pop(0), L.pop(0), ascending))
			#beende die Schleife
			break
		#wenn es mehr als zwei Elemnte zu soriteren gibt
		else:
			#fuege die ersten beiden geordnet zusammen
			L.append(Vereine(L.pop(0), L.pop(0), ascending))
		print()

	print(trenner, "\n")


#Zerteilen einer Liste in zwei gleich große Teile 
#Gibt die zerteilten Listen zurueck(als Tupel)
def Zerteile(_liste):
	#Mitte der Liste bestimmen
	haelfte = len(_liste)//2
	print("Zerteilen von:", _liste, "\n -->", _liste[:haelfte], ",", _liste[haelfte:])
	#rueckgabe der beiden Listen Haelften als Tupel
	return _liste[:haelfte], _liste[haelfte:]

#Vereinen von zwei SORTIERTEN Listen
#Gibt die vereinte Liste zurück
def Vereine(_l1, _l2, _ascending):
	print("vereine und sortiere", _l1, ",", _l2, ":")

	mergedList = []
	#solange eine der beiden Listen nicht leer ist
	while len(_l1) != 0 or len(_l2) != 0:
		#wenn erste Liste leer 
		if(len(_l1) == 0 and len(_l2) != 0):
			#Rest der 2. Liste an 1. Liste anfuegen
			mergedList.extend(_l2)
			_l2 = []
			continue
		#wenn zweite Liste leer
		if(len(_l1) != 0 and len(_l2) == 0):
			#Rest der 1. Liste an 2. anfuegen
			mergedList.extend(_l1)
			_l1 = []
			continue

		#Falls beide Listen noch Elemente enthalten	
		#1. Element der 1. Liste < 1. Element der 2. Liste...
		if(_l1[0] < _l2[0]):
			#wenn aufsteigend sortiert werden soll dann...
			if(_ascending):
				#...Das Element der 1. Liste anfuegen
				mergedList.append(_l1.pop(0))
			#wenn absteigend sortiert werden soll dann...
			else:
				#...Das Element der 2. Liste anfuegen
				mergedList.append(_l2.pop(0))
		#...sonst
		else:
			#wenn aufsteigend sortiert werden soll dann...
			if(_ascending):
				#...Das Element der 2. Liste anfuegen
				mergedList.append(_l2.pop(0))
			#wenn absteigend sortiert werden soll dann...
			else:
				#...Das Element der 1. Liste anfuegen
				mergedList.append(_l1.pop(0))

	#Ausgabe und Rueckgabe der vereinten Liste
	print(" -->", mergedList)
	return mergedList


#############################################
### QUICKSORT ###############################
#############################################
def quickSort(L, ascending = True):
	print('Quicksort, Parameter L:')
	print(L, "\n")

	if(ascending):
		print("SORTIERE AUFSTEIGEND...")
	else:
		print("SORTIERE ABSTEIGEND...")
	print(trenner)

	# Hier sollte Ihre Implementierung des inplace Quicksort-Verfahrens stehen.

	#falls die Liste nur ein/kein Element beinhaltet ist sie schon geordnet
	if(len(L) <= 1):
		return

	#zerteilen der Liste bis in den Basisfall
	Teile(L, 0, len(L) - 1, ascending)

def Teile(L, low, high, ascending):
	("Teile: ", L)
	#BESTIMMEN DES PIVOT ELEMENTES
	pivot = Pivot(L, low, high)
	#AUSGABE: [LISTE] PIVOT
	print("pivot von: ", L[low:(high+1)], "ist: ", pivot[0], "\n")
	pivotPosition = pivot[1]

	#Start der linken und ende der rechten Teilliste
	i = low
	j = high
	#alle Elemente auf der 'rechten' Seite des Pivot auf 'Pivot < Element' ueberpruefen
	while(j > pivotPosition):
		#falls Pivot > als das j-te Element
		if((L[j] < pivot[0] and ascending) or (L[j] > pivot[0] and not ascending)):
			#unterschieldiche Ausgabe je nachdem absteigend oder aufsteigend sortiert werden soll
			if(ascending):
				print(L[j], "ist kleiner als ", pivot[0])
			else:
				print(L[j], "ist groesser als ", pivot[0])
			#das Elemente an den Anfang der Teilliste schieben
			L.insert(low, L.pop(j))
			#die position des pivots ist um eins verschieben
			pivotPosition += 1
			#start der noch nicht ueberprueften linken Teilliste verschieben
			i += 1
			#AUSGABE: [LINKE TEILLISTE] PIVOT [RECHTE TEILLISTE]
			print(" -->", L[low:pivotPosition],L[pivotPosition], L[(pivotPosition+1):(high+1)], "\n")
			continue
		#Element > Pivot -> naechstes Element ueberpruefen
		j -= 1

	#alle Elemente auf der 'linken' Seite des Pivot auf 'Pivot > Element' ueberpruefen
	while(i < pivotPosition):
		#falls Pivot < als das i-te Element
		if((L[i] > pivot[0] and ascending) or (L[i] < pivot[0] and not ascending)):
			#unterschieldiche Ausgabe je nachdem absteigend oder aufsteigend sortiert werden soll
			if(ascending):
				print(L[i], "ist groesser als ", pivot[0])
			else:
				print(L[i], "ist kleiner als ", pivot[0])
			#das Elemente an das Ende der Teilliste schieben
			L.insert(high, L.pop(i))
			#die position des pivots ist um eins verschieben
			pivotPosition -= 1
			#Ende der noch nicht ueberprueften rechten Teilliste verschieben
			j -= 1
			#AUSGABE: [LINKE TEILLISTE] PIVOT [RECHTE TEILLISTE]
			print(" -->", L[low:pivotPosition],L[pivotPosition], L[(pivotPosition+1):(high+1)], "\n")
			continue
		#Element < Pivot -> naechstes Element ueberpruefen
		i += 1

	print("Teilliste 'sortiert': ", L[low:pivotPosition],L[pivotPosition], L[(pivotPosition+1):(high+1)])
	print(trenner)

	#falls noch mehr als ein Element auf der 'linken' Seite des pivot stehen 
	if(abs(low - i) > 1):
		#rekursiver Aufruf fuer den linken Teil
		Teile(L, low, i - 1, ascending)
	#falls noch mehr als ein Element auf der 'rechten' Seite des pivot stehen 
	if(abs(j - high) > 1):
		#rekursiver Aufruf fuer den rechten Teil
		Teile(L, j + 1, high, ascending)

#BESTIMMEN DES PIVOT ELEMENTES UND DESSEN POSITION
def Pivot(L, low, high):
	#laenge der Teilliste
	laenge = abs(low-high) + 1
	#Mitte der Teilliste bestimmen
	haelfte = (math.ceil(laenge / 2) - 1)

	#Anfang, Mitte, Ende bestimmen und aufsteigend sortieren
	#--> mittleres Element = Median
	return (sorted([(L[low], low), (L[low + haelfte], low + haelfte), (L[high], high)]))[1]

#############################################

#zufaellige Liste zum Testen
def ErschaffeZufaelligeListe(laenge, nachkommastellen):
	zufall = []
	for i in range(laenge):
		zufall.append(round(random.uniform(0, 1000), nachkommastellen))
	return zufall


# Hier ist ein Testfall:
liste1 = ErschaffeZufaelligeListe(10, 3)
#list([3, 2, -1, 9, 17, 4, 1, 0])
liste2 = ErschaffeZufaelligeListe(10, 1)
#list([3.14159 , 1./127, 2.718 , 1.618 , -23., 3.14159])  

mergeSort(liste1, False)
quickSort(liste2, False)

print('sortiert:')
print(liste1)
print(liste2)

# Das Ergebnis sollten folgende Liste sein:
# [-1, 0, 1, 2, 3, 4, 9, 17]
# [3.14159, 3.14159, 2.718, 1.618, 0.007874015748031496, -23.0]
