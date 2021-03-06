__author__ = 'Dario'

import tkinter, tkinter.messagebox
from random import randint

import time

fieldSizeX = 0
fieldSizeY = 0
ameisenAnzahl = 0
nestPositionX = 0
nestPositionY = 0
nestWidthX = 0
nestWidthY = 0
futterquellen = 0
futterProQuelle = 0
verdunstungszeit = 0


allAnts = []
allFutterquellen = []
#allPlaces[y][x]
allPlaces = []

#DAS CANVAS ELEMENT
w = tkinter.Canvas

simulate = False


def standardSettings():
    tmp = len(widthEntry.get())
    widthEntry.delete(0, tmp)
    widthEntry.insert(0, "300")

    tmp = len(heightEntry.get())
    heightEntry.delete(0, tmp)
    heightEntry.insert(0, "300")

    tmp = len(ameisenEntry.get())
    ameisenEntry.delete(0, tmp)
    ameisenEntry.insert(0, "2")

    tmp = len(nestpositionXEntry.get())
    nestpositionXEntry.delete(0, tmp)
    nestpositionXEntry.insert(0, "150")

    tmp = len(nestpositionYEntry.get())
    nestpositionYEntry.delete(0, tmp)
    nestpositionYEntry.insert(0, "150")

    tmp = len(futterquellenEntry.get())
    futterquellenEntry.delete(0, tmp)
    futterquellenEntry.insert(0, "3")

    tmp = len(verdunstungszeitEntry.get())
    verdunstungszeitEntry.delete(0, tmp)
    verdunstungszeitEntry.insert(0, "10")

    tmp = len(nestbreiteXEntry.get())
    nestbreiteXEntry.delete(0, tmp)
    nestbreiteXEntry.insert(0, "50")

    tmp = len(nestbreiteYEntry.get())
    nestbreiteYEntry.delete(0, tmp)
    nestbreiteYEntry.insert(0, "50")

    tmp = len(futterProQuelleEntry.get())
    futterProQuelleEntry.delete(0, tmp)
    futterProQuelleEntry.insert(0, "10")


def confirm():
    global fieldSizeX, fieldSizeY, ameisenAnzahl, nestPositionX, nestPositionY, futterquellen, verdunstungszeit, futterProQuelle, nestWidthX, nestWidthY


    fieldSizeX = int(widthEntry.get())
    fieldSizeY = int(heightEntry.get())
    ameisenAnzahl = int(ameisenEntry.get())
    nestPositionX = int(nestpositionXEntry.get())
    nestPositionY = int(nestpositionYEntry.get())
    futterquellen = int(futterquellenEntry.get())
    verdunstungszeit = int(verdunstungszeitEntry.get())
    futterProQuelle = int(futterProQuelleEntry.get())
    nestWidthX = int(nestbreiteXEntry.get())
    nestWidthY = int(nestbreiteYEntry.get())

    #print ("ALLE WERTE SIND KORREKT!")

    # ALLE Eingabefelder und Labelsloeschen aus dem Start Menu loeschen
    destroyAllStartMenuGUIs()

    # Das Fenster mit der GUI creieren
    createAntGUI()


def close():
    mainWindow.destroy()


def destroyAllStartMenuGUIs():
    widthEntry.destroy()
    widthLabel.destroy()
    heightEntry.destroy()
    heightLabel.destroy()
    ameisenEntry.destroy()
    ameisenLabel.destroy()
    nestpositionXEntry.destroy()
    nestpositionXLabel.destroy()
    nestpositionYEntry.destroy()
    nestpositionYLabel.destroy()
    futterquellenEntry.destroy()
    futterquellenLabel.destroy()
    verdunstungszeitEntry.destroy()
    verdunstungszeitLabel.destroy()
    standardBtn.destroy()
    confirmBtn.destroy()
    userInputContainer.destroy()


def createAntGUI():
    global fieldSizeX
    global fieldSizeY
    global ameisenAnzahl
    global nestPositionX
    global nestPositionY
    global futterquellen
    global verdunstungszeit
    global w, futterProQuelle, nestWidthX, nestWidthY


    # Canvas
    w = tkinter.Canvas(mainWindow,
                       width=fieldSizeX,
                       height=fieldSizeY)
    w.configure(background="grey")
    w.pack()

    # DAS NEST
    w.create_rectangle(nestPositionX, nestPositionY,
                       nestPositionX + nestWidthX, nestPositionY + nestWidthY,
                       fill="brown")

    # NEST SCHRIFTZUG
    w.create_text(nestPositionX + (nestWidthX / 2),
                  nestPositionY + (nestWidthY / 2),
                  text="Nest")

    # DIE FUTTERQUELLEN
    for i in range(0, futterquellen):
        allFutterquellen.append(Futterquelle(fieldSizeX, fieldSizeY))

        w.create_rectangle(allFutterquellen[i].posX, allFutterquellen[i].posY,
                           allFutterquellen[i].posX + 25, allFutterquellen[i].posY + 25,
                           fill="red", )

        w.create_text(allFutterquellen[i].posX + 12.5,
                      allFutterquellen[i].posY + 12.5,
                      text=str(allFutterquellen[i].portionen), tag="futterquelleText")

    # AMEISENANZAHL ANZEIGE
    antInNest = tkinter.Label(mainWindow, text="Ameisen")
    antInNest.pack(side="right")
    antInNestNumber = tkinter.Label(mainWindow, text=str(ameisenAnzahl))
    antInNestNumber.pack(side="right")

    for y in range(0, fieldSizeX):

        allPlaces.append([])

        for x in range(0, fieldSizeY):
            allPlaces[y].append(Felder(x, y))

    # Die Ameisen spawnen
    spawnAnts()


    looper()


def spawnAnts():
    global ameisenAnzahl
    global nestPositionX
    global nestPositionY
    global allAnts, counter, simulate


    for i in range(0, ameisenAnzahl):
        print ("ant", i)
        #die i'ste Ameise der allAnts Liste hinzufügen
        allAnts.append(Ant(nestPositionX, nestPositionY))

        #die i'ste Ameise zeichnen
        w.create_rectangle(allAnts[i].antPosX, allAnts[i].antPosY,
                           allAnts[i].antPosX + 10, allAnts[i].antPosY + 10,
                           fill="green", tag="ant")


def looper():
    global simulationSpeed

    #Die Simulation für die Ameisen
    for i in range (0, 10000):


        w.delete("ant")


        #alle Ameisen zeichnen
        for i in range(0, ameisenAnzahl):

            w.create_rectangle(allAnts[i].antPosX, allAnts[i].antPosY,
                                       allAnts[i].antPosX + 1, allAnts[i].antPosY + 1,
                                       outline="red", tag="ant")
            #print (allAnts[i].antPosX)
            #print (allAnts[i].antPosY)

        '''
        for y in range (0, fieldSizeY):

            for x in range (0, fieldSizeX):

                if allPlaces[y][x].pheromonKonz > 0:
                    w.create_rectangle(allPlaces[y][x].posX, allPlaces[y][x].posY,
                                       allPlaces[y][x].posX + 1, allPlaces[y][x].posY + 1,
                                       outline="pink")
        '''

        #neu Berechnung der Ameisenpositionen
        antAI()
        #Canvas updaten
        w.update_idletasks()


def antAI():
    global nestPositionX, nestPositionY, futterquellen
    foundPheromon = False

    if len(allPlaces) != 0:

        for i in range(0, ameisenAnzahl):

            #ueberpruefen ob um die Ameise herum ein Feld Pheromone hat
            checkIfPheromonIsNear(i)


            # wenn die Ameise kein Futter hat und keine Pheromonspur neben sich hat --> zufällig in eine Richtung bewegen soll
            if (allAnts[i].antHasFood == False) and (foundPheromon == False):

                randomAntMovement (i)


            # wenn die Ameise Futter hat und direkt zum Nest lufen soll
            elif (allAnts[i].antHasFood == True):

                backToNestAntMovement(i)


            # ueberpruefen ob die Ameise auf einer Futterquelle steht
            for j in range(0, futterquellen):

                checkIfAntIsOnAFutterguelle (i, j)


            # ueberpruefen ob die Ameisen auf dem Nest steht mit Futter
            if (nestPositionX <= allAnts[i].antPosX <= nestPositionX + nestWidthX) and (
                nestPositionY <= allAnts[i].antPosY <= nestPositionY + nestWidthY) and \
                (allAnts[i].antHasFood == True):


                allAnts[i].antHasFood = False
                allAnts[i].pheromonStrength = 0


#DIE AMEISEN BEWEGUNGSMOEGLICHKEITEN
def checkIfPheromonIsNear (i):

    if (allPlaces[allAnts[i].antPosY - 1][allAnts[i].antPosX].pheromonKonz > 0):

        print ("JO PHEROMON IN NORTH!!!")


def randomAntMovement (i):

    randomNr = randint(0, 3)

    # NORDEN
    if (randomNr == 0) and ((allAnts[i].antPosY - 1) != (nestPositionY + nestWidthY)):
        allAnts[i].antPosY -= 1


    # OSTEN
    elif (randomNr == 1) and ((allAnts[i].antPosX + 1) != (nestPositionX)):
        allAnts[i].antPosX += 1


    # SÜDEN
    elif (randomNr == 2) and ((allAnts[i].antPosY + 1) != (nestPositionY)):
        allAnts[i].antPosY += 1


    # WESTEN
    elif (randomNr == 3) and ((allAnts[i].antPosX - 1) != (nestPositionX + nestWidthX)):
        allAnts[i].antPosX -= 1


def backToNestAntMovement (i):

    tmpX = allAnts[i].antPosX
    tmpY = allAnts[i].antPosY

    #schon auf der breite des Nestes ---> x
    if (nestPositionX < allAnts[i].antPosX < nestPositionX + nestWidthX):
        print ("x")

    #nach rechts zum Nest ---> x++
    elif (allAnts[i].antPosX < nestPositionX):

        allAnts[i].antPosX += 1

    #nach links zum Nest ---> x--
    elif (nestPositionX + nestWidthX < allAnts[i].antPosX):

        allAnts[i].antPosX -= 1

    #schon auf der hoehe des Nestes ---> y
    elif (nestPositionY < allAnts[i].antPosY < nestPositionY + nestWidthY):
        print ("y")

    #nach unten zum Nest ---> y++
    elif (allAnts[i].antPosY < nestPositionY):

        allAnts[i].antPosY += 1

    #nach oben zum Nest
    elif (nestPositionY + nestWidthY < allAnts[i].antPosY):

        allAnts[i].antPosY -= 1

    #die vorherige position der Ameise mit einem Duftpunkt versehen
    allPlaces[tmpY][tmpX].pheromonKonz += 1


def checkIfAntIsOnAFutterguelle (i, j):

    if (allFutterquellen[j].posX <= allAnts[i].antPosX <= allFutterquellen[j].posX + 25) and (
        allFutterquellen[j].posY <= allAnts[i].antPosY <= allFutterquellen[j].posY + 25) and (
        allAnts[i].antHasFood == False) and (
        allFutterquellen[j].portionen > 0):


        allFutterquellen[j].portionen -= 1
        allAnts[i].antHasFood = True


        #die Anzeigen, wie viel Futter noch in den Futterquellen sind loeschen
        w.delete("futterquelleText")

        #alle Anzeigen wie viel Futter noch für die Futterquellen da sind neu schreiben
        for l in range(0, futterquellen):
            w.create_text(allFutterquellen[l].posX + 12.5,
                            allFutterquellen[l].posY + 12.5,
                            text=str(allFutterquellen[l].portionen), tag="futterquelleText")


class Ant:
    def __init__(self, nestPositionX, nestPositionY):
        global nestWidthX, nestWidthY, fieldSizeX

        tmp = randint(0, 3)
        # print (tmp)
        self.antHasFood = False


        if tmp == 0:
            # print ("NORDEN")
            self.antPosX = nestPositionX + randint(0, 49)
            self.antPosY = nestPositionY - 5

        elif tmp == 1:
            # print ("OSTEN")
            self.antPosX = nestPositionX + nestWidthX
            self.antPosY = nestPositionY + randint(0, 49)

        elif tmp == 2:
            # print ("SÜDEN")
            self.antPosX = nestPositionX + randint(0, 49)
            self.antPosY = nestPositionY + nestWidthY

        elif tmp == 3:
            # print ("WESTEN")
            self.antPosX = nestPositionX - 5
            self.antPosY = nestPositionY + randint(0, 49)


class Futterquelle:
    def __init__(self, fieldSizeX, fieldSizeY):
        self.portionen = 50

        self.posX = randint(0, fieldSizeX - 25)
        self.posY = randint(0, fieldSizeY - 25)


class Felder:
    def __init__(self, placePosX, placePosY):
        self.pheromonKonz = 0
        self.posX = placePosX
        self.posY = placePosY




##########################################################################
# Das Hauptfenster
mainWindow = tkinter.Tk()
mainWindow.wm_title("Ameisenfutter")
# der Knopf um das Programm zu beenden
endBtn = tkinter.Button(mainWindow, text="close", command=close)
endBtn.pack(side="right", anchor="s")

# der Container fuer die Eingabe Felder
userInputContainer = tkinter.Frame(mainWindow)
userInputContainer.pack(side="left", anchor="n")

# die Eingabefelder fuer die benutzerdefinierten Werte
# 1 WIDTH
widthEntry = tkinter.Entry(userInputContainer, text="BREITE")
widthEntry.grid(row=1, column=1)
widthLabel = tkinter.Label(userInputContainer, text="breite")
widthLabel.grid(row=1, column=2)
# 2 HEIGHT
heightEntry = tkinter.Entry(userInputContainer, text="HOEHE")
heightEntry.grid(row=2, column=1)
heightLabel = tkinter.Label(userInputContainer, text="hoehe")
heightLabel.grid(row=2, column=2)
# 3 AMEISEN
ameisenEntry = tkinter.Entry(userInputContainer, text="AMEISEN")
ameisenEntry.grid(row=3, column=1)
ameisenLabel = tkinter.Label(userInputContainer, text="ameisen")
ameisenLabel.grid(row=3, column=2)
# 4 NESTPOSITION X
nestpositionXEntry = tkinter.Entry(userInputContainer, text="NESTPOSITION X")
nestpositionXEntry.grid(row=4, column=1)
nestpositionXLabel = tkinter.Label(userInputContainer, text="nestposition X")
nestpositionXLabel.grid(row=4, column=2)
# 5 NESTPOSITION Y
nestpositionYEntry = tkinter.Entry(userInputContainer, text="NESTPOSITION Y")
nestpositionYEntry.grid(row=5, column=1)
nestpositionYLabel = tkinter.Label(userInputContainer, text="nestposition Y")
nestpositionYLabel.grid(row=5, column=2)
# 6 FUTTERQUELLEN
futterquellenEntry = tkinter.Entry(userInputContainer, text="FUTTERQUELLEN")
futterquellenEntry.grid(row=6, column=1)
futterquellenLabel = tkinter.Label(userInputContainer, text="futterquellen")
futterquellenLabel.grid(row=6, column=2)
# 7 VERDUNSTUNGSZEIT
verdunstungszeitEntry = tkinter.Entry(userInputContainer, text="VERDUNSTUNGSZEIT")
verdunstungszeitEntry.grid(row=7, column=1)
verdunstungszeitLabel = tkinter.Label(userInputContainer, text="verdunstungszeit")
verdunstungszeitLabel.grid(row=7, column=2)

# 8 Nestbreite X
nestbreiteXEntry = tkinter.Entry(userInputContainer, text="NESTBREITE X")
nestbreiteXEntry.grid(row=8, column=1)
nestbreiteXLabel = tkinter.Label(userInputContainer, text="nestbreite X")
nestbreiteXLabel.grid(row=8, column=2)
# 9 Nestbreite Y
nestbreiteYEntry = tkinter.Entry(userInputContainer, text="NESTBREITE Y")
nestbreiteYEntry.grid(row=9, column=1)
nestbreiteYLabel = tkinter.Label(userInputContainer, text="nestbreite Y")
nestbreiteYLabel.grid(row=9, column=2)
# 10 Futter pro Quelle
futterProQuelleEntry = tkinter.Entry(userInputContainer, text="FUTTER PRO QUELLE")
futterProQuelleEntry.grid(row=10, column=1)
futterProQuelleLabel = tkinter.Label(userInputContainer, text="futter pro quelle")
futterProQuelleLabel.grid(row=10, column=2)

# 11 der Knopf um die Entries mit den standard Werten zu fuellen
standardBtn = tkinter.Button(userInputContainer, text="    Standard Settings    ", command=standardSettings)
standardBtn.grid(row=11, column=1)

# der Knopf um die benutzerdefinierten Werte zu bestaetigen
confirmBtn = tkinter.Button(mainWindow, text="confirm", command=confirm)
confirmBtn.pack(side="bottom")
# Der mainLoop
mainWindow.mainloop()



##########################################################################

