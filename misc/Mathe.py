import math
import matplotlib
import matplotlib.pyplot as plt
import numpy as np



class mathe_III:

    def __init__(self):
        pass

    @classmethod
    def Trapezregel(self, f, a, b, N):
        """
        Approximiert das Integral der Funktion f in dem Intervall (a, b) durch die Trapezregel.
        Dabei wird die Funktion in N-Trapeze unterteilt.

        Args:
            f    : die zu untersuchende funktion
            a    : Intervall anfang
            b    : Intervall ende
            N    : anzahl der Streifen
        """

        #die breite eines Trapezes
        h  = math.fabs((b - a) / N)
        #Das "Anfangs"-trapez
        I = f(a) / 2
        #alle "Mittel"-trapeze
        for i in range(1, N):
            I += f(a + i * h)
        #Das "End"-trapez
        I += f(b) / 2
        #mit der breite eines streifen multiplizieren
        I *= h

        return I
    @classmethod
    def Trapezregel_max_delta(self, I, f, a, b, delta):
        """
        Sucht nach der kleinsten Unterteilung des Intervalls damit der Fehler der Trapezregel < delta gilt. 

        Args:
            I       : Der Integralwert mit dem verglichen werden soll.
            f       : Die durch Anwendung der Trapezregel zu approximierende Funktion
            a       : Untere Intervallgrenze 
            b       : Obere Intervallgrenze
            delta   : Die maximale Abweichung von dem gegebenen Integralwert.
        """

        N = 1
        I_approximated = 0

        while(math.fabs(I - I_approximated) > delta):
            I_approximated = mathe_III.Trapezregel(f, a, b, N)

            N += 1

        return N
    @classmethod
    def plotte_2D_kurve(self, e_1, e_2, a, b, s, l):
        """
        e_1 : Die funktion fuer die x-werte
        e_2 : die funktion fuer die y-werte
        a   : Intervallanfang
        b   : Intervallende
        s   : schrittweite
        l   : label der kurve in dem plot
        """

        s_1 = [e_1(t) for t in np.arange(a, b, s)]
        s_2 = [e_2(t) for t in np.arange(a, b, s)]

        plt.plot(s_1, s_2, label=l)
        
        return plt
    @classmethod
    def plotte_vektor(self, x_l, y_l, x_v, y_v):
        """
        """

        plt.quiver(x_l, y_l, x_v, y_v)
        
        return plt
        

    @classmethod
    def blatt_3__nr_2(self):

        f = lambda x : math.sin(x)

        a = 0
        b = math.pi

        #a
        print(self.Trapezregel(f, a, b, 1))

        #b
        print(self.Trapezregel(f, a, math.pi/2, 1))
        print(self.Trapezregel(f, math.pi/2, b, 1))

        #c
        print(self.Trapezregel(f, a, b, 4))

        #d
        print(self.Trapezregel, a, b, 8)

    @classmethod
    def blatt_4__nr_1(self):

        f = lambda x : 1/x

        print(self.Trapezregel_max_delta(math.log2(2), f, 1, 2, 10**(-8)))

    @classmethod
    def blatt_5__nr_1(self):
        
        #a)  plotte newton knoten
        e_1 = lambda t : ((t ** 2) - 1)
        e_2 = lambda t : ((t ** 3) - t)
        plt = self.plotte_2D_kurve(e_1, e_2, -2, 2, 0.01, "newtonknoten")

        #b)  draw vektors
        plt = self.plotte_vektor([0, -1, 0], [0, 0, 0], [-2, 0, 2], [2, -1, 2])

        #c)  plotte norm des geschw.-vektors
        x = [i for i in np.arange(-2, 2, 0.01)]
        f = (lambda t : math.sqrt((((3*t**2) - 1)**2) + ((2*t)**2)))
        y = [f(t) for t in np.arange(-2, 2, 0.01)]
        plt.plot(x, y, label="Norm des geschw.-vektors")

        plt.gca().set_xlim(-12, 12)
        plt.gca().set_ylim(-12, 12)
        
        plt.legend()
        plt.grid()
        plt.show()
    @classmethod
    def blatt_5__nr_2(self):

        a = -2*math.pi
        b = 2*math.pi

        #a) plotte kurve
        e_1 = lambda t : (math.e ** (1 / 2*math.pi * t)) * math.cos(t)
        e_2 = lambda t : (math.e ** (1 / 2*math.pi * t)) * math.sin(t)
        plt = self.plotte_2D_kurve(e_1, e_2, a, b, 0.01, "l(t)")

        plt.xscale("symlog", linthreshx=0.001)
        plt.yscale("symlog", linthreshy=0.001)

        plt.axhline(0, color="black")
        plt.axvline(0, color="black")
        plt.show()

        #a) laenge des kurvenstueckes
        f = lambda t : math.sqrt(( ( math.e**( (1/ (2*math.pi) ) * t)) * -math.sin(t) )**2 +\
                                 ( ( math.e**( (1/ (2*math.pi) ) * t)) *  math.cos(t) )**2)
        print(self.Trapezregel(f, -2*math.pi, 2*math.pi, 200000))
        
    @classmethod
    def blatt_5__nr_3(self):
        #keine aufgabe -> plot der Kurve
        e_1 = lambda t : t
        e_2 = lambda t : t**2
        plt = self.plotte_2D_kurve(e_1, e_2, 0, 1, 0.01, "f(t) = [t, t^2]")

        plt.show()

        #3)
        print(self.Trapezregel(lambda t : math.sqrt(1 + (2*t)**2), 0, 1, 2000))

    @classmethod
    def blatt_6__nr_2(self):
        
        e_1 = lambda t : math.cos(t) ** 3
        e_2 = lambda t : math.sin(t) ** 3
        plt = self.plotte_2D_kurve(e_1, e_2, 0, 2 * math.pi, 0.01, "f(t) = [cos^3 t, sin^3 t]")

        plt.show()

        f = lambda t : math.sqrt(math.pow(-3 * math.pow(math.cos(t), 2) * math.sin(t), 2) +
                                 math.pow( 3 * math.pow(math.sin(t), 2) * math.cos(t), 2))
        print(self.Trapezregel(f, 0, 2 * math.pi, 20000))




if __name__ == "__main__":

    #mathe_III.blatt_3__nr_2()

    #mathe_III.blatt_4__nr_1()

    #mathe_III.blatt_5__nr_1()
    #mathe_III.blatt_5__nr_2()
    #mathe_III.blatt_5__nr_3()

    mathe_III.blatt_6__nr_2()
