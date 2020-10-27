


def createChart(items , MAX):

	print("createChart")
	print("Maximales Gewicht:", MAX, " ; Items:", items, "\n")

	#Die Tabelle erstellen und mit (0, 0) füllen
	Chart = [[(0, 0) for x in range(MAX + 1)] for x in range(len(items) + 1)]

	#item bestimmen
	for i in range(len(items) + 1):
		#momentanes Maximalgewicht bestimmen
		for g in range(MAX + 1):
			#wenn kein item oder kein gewicht genommen werden darf
			if i==0 or g==0:
				continue
			#falls das Gewicht des letzten item <= momentanen Maximalgewicht ist
			elif items[i-1][1] <= g:
				#falls Wert des letzen item + Optimal mgl. Wert bis einem Item weniger
				# > Optimal mgl. Wert bis einem Item weniger
				if(items[i-1][0] + Chart[i-1][g - items[i-1][1]][0] > Chart[i-1][g][0]):
					#optimal mgl. Wert = Wert des letzen item + Optimal mgl. Wert bis einem Item weniger
					Chart[i][g] = (items[i-1][0] + Chart[i-1][g - items[i-1][1]][0], 1)
				#falls Wert des letzen item + Optimal mgl. Wert bis einem Item weniger
				# <= Optimal mgl. Wert bis einem Item weniger
				else:
					#das Optimum ändert sich nicht 
					#im Vergleich zum Optimum bis ein Item weniger
					Chart[i][g] = (Chart[i-1][g][0], 0)
			#falls das Gewicht des letzten item > momentanen Maximalgewicht ist
			else:
				#das Optimum ändert sich nicht
				Chart[i][g] = (Chart[i-1][g][0], 0)

	return Chart


def bestChoice( chart , items ):
	
	print("\nbestChoice")

	#die Liste in der gespeichert wird
	#welche items mitgenommen(1) und NICHT mitgenommen(0) werden
	bestSet = []
	#das maximale Gewicht das getragen werden kann
	g = len(chart[0]) - 1
	#wie viele items es gibt
	i = len(chart) - 1

	#solange es noch items zu überprüfen gibt
	while(i > 0):
		print("überprüfe chart["+str(i)+"]"+"["+str(g)+ "]")
		#falls das item an der Stelle [i][g] für das optimalset mitgenommen wird 
		if(chart[i][g][1] == 1):
			#das item wird mitgenommen(in Liste eintragen)
			bestSet.insert(0, 1)
			#das noch zulässige Gewicht reduzieren
			#um den nächsten Tabellen eintrag zu ermitteln
			g -= items[i - 1][1]
			print("--> items[" + str(i-1) + "] =", items[i - 1], "wird mitgenommen")
		#falls das item an der Stelle [i][g] für das optimalset NICHT mitgenommen wird 
		else:
			#das item wird NICHT mitgenommen(in Liste eintragen)
			bestSet.insert(0, 0)
			print("--> items[" + str(i-1) + "] =",items[i - 1], "wird nicht mitgenommen")
		
		#es darf nur noch ein item weniger mitgenommen werden
		i -= 1
	return bestSet

#gibt einen chart Zeile für Zeile aus
def PrintChart(_set):
	for i in _set:
		print(i)

# Hier ist ein Testfall:

items = [(3,4),(1,1),(4,5),(3,4),(2,2)]
#items = [(12, 4), (10, 6), (8, 5), (11, 7),(14, 3),(7, 1),(9, 6)]
maxWeight = 8
T = createChart( items , maxWeight )
PrintChart(T)
#jetzt sollte T folgendes sein:
#[[(0, 0) (0, 0) (0, 0) (0, 0) (0, 0) (0, 0) (0, 0) (0, 0) (0, 0)],
# [(0, 0) (0, 0) (0, 0) (0, 0) (3, 1) (3, 1) (3, 1) (3, 1) (3, 1)],
# [(0, 0) (1, 1) (1, 1) (1, 1) (3, 0) (4, 1) (4, 1) (4, 1) (4, 1)],
# [(0, 0) (1, 0) (1, 0) (1, 0) (3, 0) (4, 0) (5, 1) (5, 1) (5, 1)],
# [(0, 0) (1, 0) (1, 0) (1, 0) (3, 0) (4, 0) (5, 0) (5, 0) (6, 1)],
# [(0, 0) (1, 0) (2, 1) (3, 1) (3, 0) (4, 0) (5, 0) (6, 1) (7, 1)]]

#erste Komponente des letzten Tabelleneintrags:
bestValue = T[-1][-1][0] #sollte den Maximalwert angeben ( = 7 )
print("\nbestValue: ", bestValue)

L = bestChoice( T , items)
print("\n" + str(L))
#sollte [0, 1, 1, 0, 1] ergeben (i.a. nicht eindeutig, in diesem Bsp. schon)


