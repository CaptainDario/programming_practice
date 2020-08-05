from tkinter import *
import copy


#D:\Dario\BW_Info\kassiopeia
#Die Datei in der die Splaten und Reihen Angaben sind
source = ""

#Inhalt der Reihen und Splaten
row = ""
column = []

#Alle weißen Felder
whiteBlocksTotal = []
whiteBlocksVisited = []


#zaelt die lines mit
i = -1

#Die Reihe und die Spalte in der Kasiupeia sich befindet
kassioColumn = 0
kassioRow = 0

#Hoehe und breite des canvas
width = 0
height = 0




def KassioPos():
    global kassioColumn
    global kassioRow
    global i
    global row
    global column
    global source


    f = open (source, "r")
    for line in f:
        #print (line)

        i += 1

        row = line.replace ("\n", "")
        column.append (row)

        if "#" in row:
            print(row)
            j = 0
            for j in range (len(row)):
                if "#" == row[j]:

                    can.create_rectangle(40 * j, 40 * i,
                                         40 * j + 41, 40 * i + 41,
                                         fill= "black")

                elif " " == row[j]:

                    whiteBlocksTotal.append([j + 1, i])

                    can.create_rectangle(40 * j, 40 * i,
                                         40 * j + 41, 40 * i + 41,
                                         fill= "white")

                else:
                    can.create_rectangle(40 * j, 40 * i,
                                         40 * j + 41, 40 * i + 41,
                                         fill= "blue")

        if "K" in row:
            kassioColumn = i
            kassioRow = row.find ("K")
            kassioRow += 1


    if kassioColumn == 0:
        print ("KEINE KASIUPEIA GEFUNDEN")

    else:
        #print (column)
        print ("kassioPos.x = ", kassioRow, ", kassioPos.y = ", kassioColumn)
        whiteBlocksTotal.sort()
        print ("whiteBlocksTotal = ", whiteBlocksTotal)

        walk ()




def walk():
    global whiteBlocksTotal
    global whiteBlocksVisited
    global kassioRow
    global kassioColumn
    i = 0

    whiteBlocksTotal.append ([kassioRow, kassioColumn])
    whiteBlocksVisited.append ([kassioRow, kassioColumn])

    while i < len(whiteBlocksVisited):

        x = whiteBlocksVisited[i][0]
        y = whiteBlocksVisited[i][1]

        #print ([x, y ])

        #ueberprueft ob der Noerdlich gelegene Block ein weisser ist
        if ([x, y - 1] in whiteBlocksTotal):

            #print("NoeRDLICH IST ES WEiss�!")

            if ([x, y - 1] not in whiteBlocksVisited):
                whiteBlocksVisited.append ([x, y - 1])


        #Überprüft ob der Nördlich gelegene Block ein weißer ist
        if ([x + 1, y] in whiteBlocksTotal):

            #print("ÖSTLICH IST ES WEIß!")

            if ([x + 1, y] not in whiteBlocksVisited):
                whiteBlocksVisited.append ([x + 1, y])


        #Überprüft ob der Nördlich gelegene Block ein weißer ist
        if ([x, y + 1] in whiteBlocksTotal):

            #print("SÜDLICH IST ES WEIß!")

            if ([x, y + 1] not in whiteBlocksVisited):
                whiteBlocksVisited.append ([x, y + 1])


        #Überprüft ob der Nördlich gelegene Block ein weißer ist
        if ([x - 1, y] in whiteBlocksTotal):

            #print("WESTLICH IST ES WEIß!")

            if ([x - 1, y] not in whiteBlocksVisited):
                whiteBlocksVisited.append ([x - 1, y])


        i += 1
        #print ("i = ", i)
        #print ("whiteBlocksVisited = ", whiteBlocksVisited)


    whiteBlocksTotal.sort()
    whiteBlocksVisited.sort()
    #print (whiteBlocksTotal)
    #print (whiteBlocksVisited)

    if (whiteBlocksTotal == whiteBlocksVisited):
        print ("Kassiopeia kann jedes Feld abfressen!!")
        asd = Label(myWindow, text = "JUHU Sie kann jedes Feld betreten!")
        asd.pack()
        canEatAll = Button (myWindow,command = canEatAllBtn
                            ,text= "Kann sie das auch wenn sie jedes Feld nur einmal betreten darf?")
        canEatAll.pack()

    else:
        print ("Kassiopeia kann nicht jedes Feld abfressen!!")
        #Textausgabe das sie nicht alle Felder abfressen kann
        cantEatAll = Label(myWindow, text = "Kassiopeis kann nicht jedes Feld abfressen!")
        cantEatAll.pack()



def okBtn():
    global source

    if dataPath.get() != "":
        source = dataPath.get()

        try :
            open (source, "r")


        except:
            print ("Datei oder Dateipfad nicht gefunden")

        else:
            print (source)
            KassioPos()



    else :
        print ("Gib einen Dateipfad an!")


#Wenn Kassiopeia alle Felder ablaufen kann, kann diese Methode gerufen werden
def canEatAllBtn():

    global whiteBlocksTotal
    global kassioRow
    global kassioColumn


    j = 0

    while j > -1:


        if j == 0:

            #alle Möglichkeiten die beim ablaufen der Wege gefunden wurden
            possibilities = []

            #StartPosition der Schildkröte
            x = kassioRow
            y = kassioColumn

            #alle weißen Blöcke temporär abspeichern
            whiteBlocksTotal_TMP = copy.deepcopy (whiteBlocksTotal)
            #und die Startposition entfernen
            whiteBlocksTotal_TMP.remove([kassioRow, kassioColumn])
            #Die Position der Schildkröte von allen weißen Blöcken entfernen
            whiteBlocksTotal.remove([kassioRow, kassioColumn])

            #der aktuelle Weg
            actualWay = [[kassioRow, kassioColumn]]



            if [x, y - 1] in whiteBlocksTotal :

                if len(actualWay) < 2  :

                    actualWay.append ([x, y - 1])

                    whiteBlocksTotal_TMP.remove ([x, y - 1])
                    #print ("WE SHOULD FIRSTLY GO NORDEN")

                else:

                    possibilities.append ([[kassioRow, kassioColumn], [x, y - 1] ])
                    #print ("NORDEN IS AT START POSSIBLE")


            if [x + 1, y] in whiteBlocksTotal :

                if len(actualWay) < 2:

                    actualWay.append ([x + 1, y])

                    whiteBlocksTotal_TMP.remove ([x + 1, y])
                    #print ("WE SHOULD FIRSTLY GO OSTEN")

                else:

                    possibilities.append ([[kassioRow, kassioColumn], [x + 1, y] ])
                    #print ("OSTEN IS AT START POSSIBLE")


            if [x, y + 1] in whiteBlocksTotal :

                if len(actualWay) < 2:

                    actualWay.append ([x, y + 1])

                    whiteBlocksTotal_TMP.remove ([x, y + 1])
                    #print ("WE SHOULD FIRSTLY GO SÜDEN")

                else:

                    possibilities.append ([[kassioRow, kassioColumn], [x, y + 1]])
                    #print ("SÜDEN IS AT START POSSIBLE")


            if [x - 1, y] in whiteBlocksTotal :

                if len(actualWay) < 2:

                    actualWay.append ([x - 1, y])

                    whiteBlocksTotal_TMP.remove ([x - 1, y])
                    #print ("WE SHOULD FIRSTLY GO WESTEN")

                else:

                    possibilities.append ([[kassioRow, kassioColumn], [x - 1, y] ])
                    #print ("WESTEN IS AT START POSSIBLE")

            #print ("actualWay = ", actualWay)
            x = actualWay [1][0]
            y = actualWay [1][1]

            #print ("whiteBLocksTotal_TMP = ", whiteBlocksTotal_TMP)
            #print ("Possibilities = ", possibilities)
            #print ("actualWay = ", actualWay)
            #print ("x = ", x)
            #print ("y = ", y)


        else:

            #alle weißen Blöcke temporär abspeichern
            whiteBlocksTotal_TMP = copy.deepcopy (whiteBlocksTotal)

            #den aktuellen weg mit dem nächsten von den possibilities gleichsetzen
            actualWay = copy.deepcopy (possibilities[j - 1])
            #[len(possibilities[j]) - 1])


            #alle schon gegangenen Positionen aus whiteBlocksTotal_TMP entfernen
            for l in actualWay :

                if l in whiteBlocksTotal_TMP:
                    whiteBlocksTotal_TMP.remove(l)

            #die Startposition an den anfang hinzufuegen
            #actualWay = [[kassioRow, kassioColumn]] + actualWay



            #print ("possibilities[j - 1] = ", possibilities[j - 1])

            x = possibilities[j - 1][len(possibilities[j - 1]) - 1][0]

            y = possibilities[j - 1][len(possibilities[j - 1]) - 1][1]

            #print ("\n")



        # solange wie es noch Moeglichkeiten gibt werden diese ausprobiert
        actualWay_TMP = copy.deepcopy (actualWay)


        while [x, y - 1] in whiteBlocksTotal_TMP or [x + 1, y] in whiteBlocksTotal_TMP or [x, y + 1] in whiteBlocksTotal_TMP or [x - 1, y] in whiteBlocksTotal_TMP :

            #print ("STARTED WHILE LOOP")
            #print ("actualWay = ", actualWay)
            i = 1
            i_TMP = i
#####NORDEN####################################################
            if [x, y - 1] in whiteBlocksTotal_TMP:

                print ("NORDEN")
                #Allen weißen Blöcken, die noch nicht begangen wurden den Nördichen abziehen...
                whiteBlocksTotal_TMP.remove([x, y - 1])


                #die Koordinaten dieses Punktes temporär abspeichern um sie dann am Ende erst mit der
                #aktuellen Position gleichzusetzten
                x_TMP = x
                y_TMP = y - 1

                #i um eins erhöhen damit klar ist das ein neuer Weg schon gefunden wurde
                i += 1


#######OSTEN#####################################################
            if [x + 1, y] in whiteBlocksTotal_TMP and i == i_TMP:

                #print ("OSTEN")
                #Allen weißen Bloecken, die noch nicht begangen wurden, den Östlichen abziehen...
                whiteBlocksTotal_TMP.remove([x + 1, y])

                #die Koordinaten dieses Punktes temporär abspeichern um sie dann am Ende erst mit der
                #aktuellen Position gleichzusetzten
                x_TMP = x + 1
                y_TMP = y

                #i um eins erhöhen damit klar ist das ein neuer Weg schon gefunden wurde
                i += 1


            #wenn der ÖSTLICHE Block in der Liste mit den noch begehbaren blöcken ist
            #aber ein anderer weg schon gegangen wird
            elif [x + 1, y] in whiteBlocksTotal_TMP and i != i_TMP:
                #print ("OSTEN_POSSIBLE")

                #actualWay Temporär mit dem möglichen Weg abspeichern
                actualWay_TMP = copy.deepcopy(actualWay)
                actualWay_TMP.append ([x + 1, y])

                possibilities.append (actualWay_TMP)





######SÜDEN######################################################
            if [x, y + 1] in whiteBlocksTotal_TMP and i == i_TMP:
                #print ("SÜDEN")
                #Allen weißen Blöcken, die noch nicht begangen wurden, den Östlichen abziehen...
                whiteBlocksTotal_TMP.remove([x, y + 1])

                #die Koordinaten dieses Punktes temporär abspeichern um sie dann am Ende erst mit der
                #aktuellen Position gleichzusetzten
                x_TMP = x
                y_TMP = y + 1

                #i um eins erhöhen damit klar ist das ein neuer Weg schon gefunden wurde
                i += 1


            #wenn der SÜDLICHE Block in der Liste ist aber ein anderer weg schon gegangen wird
            elif [x, y + 1] in whiteBlocksTotal_TMP and i != i_TMP:
                #print ("SÜDEN_POSSIBLE")


                #actualWay Temporär mit dem möglichen Weg abspeichern
                actualWay_TMP = copy.deepcopy(actualWay)
                actualWay_TMP.append ([x, y + 1])

                possibilities.append (actualWay_TMP)



######WESTEN#####################################################
            if [x - 1, y] in whiteBlocksTotal_TMP and i == i_TMP:
                # ("WESTEN")
                #Allen weissen Bloecken, die noch nicht begangen wurden, den Östlichen abziehen...
                whiteBlocksTotal_TMP.remove([x - 1, y])

                #die Koordinaten dieses Punktes temporär abspeichern um sie dann am Ende erst mit der
                #aktuellen Position gleichzusetzten
                x_TMP = x - 1
                y_TMP = y

                #i um eins erhöhen damit klar ist das ein neuer Weg schon gefunden wurde
                i += 1


            #wenn der WESTLICHE Block in der Liste ist aber ein anderer weg schon gegangen wird
            elif [x - 1, y] in whiteBlocksTotal_TMP and i != i_TMP:
                #print ("WESTEN_POSSIBLE")

                #sctualWay Temporär mit dem möglichen Weg abspeichern
                actualWay_TMP = copy.deepcopy(actualWay)
                actualWay_TMP.append ([x - 1, y])

                possibilities.append (actualWay_TMP)




######AM ENDE DES WHILE LOOPS#####################################



            x = x_TMP
            y = y_TMP
            #print ("actualPos = ", x, y)
            #... und diesen Block dem aktuellen Weg hinzufuegen
            actualWay.append([x, y])
            #print ("\n")
            #print ("possiblities = ", possibilities)
            #print ("\n")
            #print ("\n")



        j += 1

        if len(actualWay) > len(whiteBlocksTotal):

            print (actualWay)
            print ("FERTIG")
            print (actualWay [1][1])
            directionsList = []

            for l in range (0, len (actualWay)):


                if (l > 0):

                    if (actualWay [l - 1][1] < actualWay [l][1]):

                        directionsList.append ("S")
                        print (l, "SUEDEN")

                    elif (actualWay [l - 1][0] > actualWay [l][0]):

                        print (l, "WESTEN")
                        directionsList.append ("W")

                    elif (actualWay [l - 1][1] > actualWay [l][1]):

                        print (l, "NORDEN")
                        directionsList.append ("N")

                    elif (actualWay [l - 1][0] < actualWay [l][0]):

                        print (l, "OSTEN")
                        directionsList.append ("O")

            fgh = Label(myWindow, text = directionsList, fg = "yellow", bg = "black")
            fgh.pack()

            break





############################################################################
myWindow = Tk()
myWindow.wm_title("Kassiopeia sucht ihren Weg")

#container fuer den Dateipfad
dataPathContainer = Frame(myWindow)
dataPathContainer.pack(anchor= NW)

dataPath = Entry(dataPathContainer)
dataPath.pack(side= LEFT)

dataPathButton = Button (dataPathContainer, text= "OK", command = okBtn)
dataPathButton.pack(side=LEFT)


#container fuer die Bloecke
can = Canvas(myWindow, width = 600, height = 600)
can.pack(anchor= NW)


myWindow.mainloop()
############################################################################

