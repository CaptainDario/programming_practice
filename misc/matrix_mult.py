import sys


# Diese Funktion koennen Sie verwenden, um Matrizen auszugeben.
def showMatrix(m):
	for line in m:
		print('|', end='')
		i = 0
		for value in line:
		    if (i > 0):
		        print(' ', end='')
		    print(value, end='')
		    i = i + 1
		print("|")

# Implementieren Sie ab hier Ihre Loesungen:
def matMultDef(a, b):
	print("Matrixmultiplikation nach Definition")
	print('Parameter a:')
	showMatrix(a)
	print('Parameter b:')
	showMatrix(b)
    # hier soll Ihre Implementierung der Matrixmultiplikation laut Definition stehen.
	m = len(a[0])
	n = len(b)
	if(m != n):
		print("Matrizen koennen nicht multipliziert werden")
		return
	
	ergebniss = []
	zwischenergebniss = []

	tmp = 0
	str = ""
	#str2 = ""

	for x in range(0, len(a)):
		zwischenergebniss = []
		for y in range(0, len(b[0])):

			for i in range(0, n):
				tmp += a[x][i] * b[i][y]
				if(i < len(b[0]) - 1):
					str += " (" + (a[x][i]).__str__() + " * " +  (b[i][y]).__str__() + ") " + "+"
				else:
					str += " (" + (a[x][i]).__str__() + " * " +  (b[i][y]).__str__() + ") " + "= "

			print(str + tmp.__str__())
			str = ""
			zwischenergebniss.append(tmp)
			tmp = 0

		ergebniss.append(zwischenergebniss)
	
	result = ergebniss 
	return ergebniss

def matMultDC(a, b):
	print("Matrixmultiplikation nach Teile und herrsche")
	print('Parameter a:')
	showMatrix(a)
	print('Parameter b:')
	showMatrix(b)
    # hier soll Ihre Implementierung der Matrixmultiplikation nach dem Paradigma "Teile und Herrsche"
	result = Divide(a, b)
	return result

def Divide(a, b):
	m = len(a[0])
	n = len(b)
	if(m != n):
		print("Matrizen koennen nicht multipliziert werden")
		return

	print("\nBEGINNE ZERTEILEN...\n")

	list = []
	tmp = []
	cnt = 0;
	#zerteile Matrix b in Vektoren der Groesse len(b)
	print("Zerteile Matrix b in Vektoren...")
	#waehle das x'te Element aus der jeweiligen Matrix Spalte
	for x in range(len(b[0])):
		#waehle Elemente aus der y'ten Zeile aus b
		for y in range(len(b)):
			#fuege sie zu Vektor zusammen
			tmp.append(b[y][x])
		for i in range(len(a)):
			#fuege Vektor zur liste mit allen Vektoren zusammen
			list.append([tmp])
		print("Vektor", x, tmp)
		tmp = []

	#zerteile Matrix a in Vektoren der Laenge len(a[0])
	print("\nZerteile Matrix a in Vektoren...")
	for i in range(0, len(list)):
		if(i < len(a)):
			print("Vektor", i, a[i])
		#fuege Vektor zu der Liste mit allen Vekotren zusammen
		list[i].append(a[i%len(a)])
		

	print("\nZerteile Vektoren in den Basisfall \n")

	list2 = []
	list3 = []
	#zerteilt die verbliebenen Vektoren weiter in die einzelnen Komponente
	#Schleife ueber alle Vektoren
	for x in range(len(list)):
		#ueber die Laenge eines Vektorenpaares
		for y in range(0, len(list[x]), 2):
			#ueber ein Vektor
			for z in range(len(list[x][y])):
				#fuege den Basisfall dem zusammen zusetzenden zu
				list3.append([list[x][y][z], list[x][y + 1][z]])
				print([list[x][y][z], list[x][y + 1][z]])
			print("\n")
			list2.append(list3)
			list3 = []

	print(list2)

	print("zerteilt \n")

	return Conquer(list2, len(a))

def Conquer(m, _a):
	print("Ergebnis zusammensetzen... \n")

	list5 = []
	for i in range(_a):
		list5.append([])
	print(a)
	tmp = 0
	#Schleife fuer jede Gruppe von Basisfaellen
	for i in range(len(m)):
		#Schleife fuer jeden Basisfall einer Gruppe
		for j in range(len(m[0])):
			#zwischenergebnis merken
			tmp += m[i][j][0] * m[i][j][1]
			print(m[i][j][0], " * ", m[i][j][1], " = ", m[i][j][0] * m[i][j][1])
		print("Matrix[", i % _a, "]", "[", i // _a, "] = ", tmp, "\n")
		#Matrixplatz belgen
		list5[(i % _a)].append(tmp)
		tmp = 0
		list4 = []

	print("Zusammengesetzt... \n", list5, "\n")

	return list5


a = [[3, 2, 1],
[1, 0, 2]]
b = [[1, 2],
[0, 1],
[4, 0]]

# Hier ist ein Testfall:
result = matMultDC(a, b)

print('berechnet:')
showMatrix(result)

print('\n')

result2 = matMultDef(a, b)

print('\n berechnet:')
showMatrix(result)

# Das Ergebnis sollte folgende Matrix sein:
# [
# [7, 8],
# [9, 2]
# ]
