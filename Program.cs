namespace Tirer_Joueur
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Bienvenu dans mon programme !");
            //declaration de variable
            int largeur;//largeur de la grille
            int hauteur;//hauteur de la grille
                        //initialisation variable
            largeur = 10;
            hauteur = 10;

            // Créer les grilles pour les deux joueurs
            string[,] grilleJoueur1 = new string[hauteur, largeur];
            string[,] grilleJoueur2 = new string[hauteur, largeur];

            // Créer les grilles de tirs (ce que chaque joueur voit de l'adversaire)
            string[,] grilleTirsJoueur1 = new string[hauteur, largeur]; // Tirs du joueur 1 sur joueur 2
            string[,] grilleTirsJoueur2 = new string[hauteur, largeur]; // Tirs du joueur 2 sur joueur 1

            //traitement
            // Placement des bateaux pour le Joueur 1
            RemplirGrille(grilleJoueur1);
            RemplirGrille(grilleTirsJoueur1);
            AfficherInfosBateaux();
            PlacerTousLesBateaux(grilleJoueur1, "JOUEUR 1");

            // Nettoyer l'écran avant que le Joueur 2 place ses bateaux
            Console.WriteLine("\n\n=== Le Joueur 1 a terminé ! ===");
            Console.WriteLine("Appuyez sur une touche pour que le Joueur 2 commence...");
            Console.ReadKey();
            Console.Clear();

            // Placement des bateaux pour le Joueur 2
            RemplirGrille(grilleJoueur2);
            RemplirGrille(grilleTirsJoueur2);
            AfficherInfosBateaux();
            PlacerTousLesBateaux(grilleJoueur2, "JOUEUR 2");

            // Commencer la phase de combat
            Console.Clear();
            Console.WriteLine("=== Que la bataille commence ! ===\n");
            Console.WriteLine("Appuyez sur une touche pour commencer...");
            Console.ReadKey();
            // Jouer la bataille
            JouerBataille(grilleJoueur1, grilleJoueur2, grilleTirsJoueur1, grilleTirsJoueur2, largeur, hauteur);
        }
        // Fonction principale de la bataille
        static void JouerBataille(string[,] grilleJ1, string[,] grilleJ2, string[,] tirsJ1, string[,] tirsJ2, int largeur, int hauteur)
        {
            bool joueur1Actif = true; // true = tour du joueur 1, false = tour du joueur 2
            bool partieEnCours = true;

            while (partieEnCours)
            {
                Console.Clear();

                if (joueur1Actif)
                {
                    Console.WriteLine("=== Tour du JOUEUR 1 ===\n");
                    Console.WriteLine("Vos tirs précédents :");
                    AfficherGrille(largeur, hauteur, tirsJ1);

                    // Tirer
                    Tirer(grilleJ2, tirsJ1, "JOUEUR 1");

                    // Vérifier si le joueur 2 a perdu
                    if (TousLesBateauxCoules(grilleJ2))
                    {
                        Console.WriteLine("\n🎉🎉🎉 JOUEUR 1 A GAGNÉ ! 🎉🎉🎉");
                        partieEnCours = false;
                    }
                }
                else
                {
                    Console.WriteLine("=== Tour du JOUEUR 2 ===\n");
                    Console.WriteLine("Vos tirs précédents :");
                    AfficherGrille(largeur, hauteur, tirsJ2);

                    // Tirer
                    Tirer(grilleJ1, tirsJ2, "JOUEUR 2");

                    // Vérifier si le joueur 1 a perdu
                    if (TousLesBateauxCoules(grilleJ1))
                    {
                        Console.WriteLine("\n🎉🎉🎉 JOUEUR 2 A GAGNÉ ! 🎉🎉🎉");
                        partieEnCours = false;
                    }
                }

                if (partieEnCours)
                {
                    Console.WriteLine("\nAppuyez sur une touche pour passer au joueur suivant...");
                    Console.ReadKey();
                    joueur1Actif = !joueur1Actif; // Changer de joueur
                }
            }

            Console.WriteLine("\nAppuyez sur une touche pour quitter...");
            Console.ReadKey();
        }

        // Fonction pour effectuer un tir
        static void Tirer(string[,] grilleAdversaire, string[,] grilleTirs, string nomJoueur)
        {
            bool tirValide = false;

            while (!tirValide)
            {
                Console.Write("\nEntrez les coordonnées de tir (ex: A1) : ");
                string coordonnee = Console.ReadLine().Trim();

                // Convertir la coordonnée
                if (coordonnee.Length < 2)
                {
                    Console.WriteLine("Coordonnée invalide !");
                    continue;
                }

                char lettreCol = char.ToUpper(coordonnee[0]);
                string numLigne = coordonnee.Substring(1);

                // Vérifier que la lettre est valide (A-J)
                if (lettreCol < 'A' || lettreCol > 'J')
                {
                    Console.WriteLine("La colonne doit être entre A et J !");
                    continue;
                }

                // Convertir la colonne (A=0, B=1, etc.)
                int colonne = lettreCol - 'A';

                // Convertir le numéro de ligne
                if (!int.TryParse(numLigne, out int ligne) || ligne < 1 || ligne > 10)
                {
                    Console.WriteLine("La ligne doit être entre 1 et 10 !");
                    continue;
                }
                ligne--; // Ajuster pour l'index du tableau (0-9)

                // Vérifier si on a déjà tiré à cet endroit
                if (grilleTirs[ligne, colonne] != "~")
                {
                    Console.WriteLine("Vous avez déjà tiré à cet endroit !");
                    continue;
                }

                tirValide = true;

                // Vérifier le résultat du tir
                string caseAdversaire = grilleAdversaire[ligne, colonne];

                if (caseAdversaire == "~")
                {
                    // Raté
                    Console.WriteLine("\n💧 MANQUÉ !");
                    grilleTirs[ligne, colonne] = "O"; // O = raté
                    grilleAdversaire[ligne, colonne] = "O";
                }
                else
                {
                    // Touché
                    char bateauTouche = caseAdversaire[0];
                    grilleTirs[ligne, colonne] = "X"; // X = touché
                    grilleAdversaire[ligne, colonne] = "X";

                    // Vérifier si le bateau est coulé
                    if (BateauCoule(grilleAdversaire, bateauTouche))
                    {
                        string nomBateau = ObtenirNomBateau(bateauTouche);
                        Console.WriteLine($"\n💥 TOUCHÉ ! 💥");
                        Console.WriteLine($"🚢 {nomBateau} COULÉ ! 🚢");
                    }
                    else
                    {
                        Console.WriteLine("\n💥 TOUCHÉ ! 💥");
                    }
                }
            }
        }

        // Fonction pour vérifier si un bateau est complètement coulé
        static bool BateauCoule(string[,] grille, char symboleBateau)
        {
            int hauteur = grille.GetLength(0);
            int largeur = grille.GetLength(1);

            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    if (grille[i, j].Length > 0 && grille[i, j][0] == symboleBateau)
                    {
                        return false; // Il reste encore une partie du bateau
                    }
                }
            }

            return true; // Le bateau est complètement coulé
        }

        // Fonction pour obtenir le nom du bateau à partir de son symbole
        static string ObtenirNomBateau(char symbole)
        {
            switch (symbole)
            {
                case 'P':
                    return "Porte-avions";
                case 'C':
                    return "Cuirassé";
                case 'R':
                    return "Croiseur";
                case 'S':
                    return "Sous-marin";
                case 'D':
                    return "Destroyer";
                default:
                    return "Bateau inconnu";
            }
        }

        // Fonction pour vérifier si tous les bateaux d'un joueur sont coulés
        static bool TousLesBateauxCoules(string[,] grille)
        {
            int hauteur = grille.GetLength(0);
            int largeur = grille.GetLength(1);

            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    string cellule = grille[i, j];
                    // Si on trouve une lettre (bateau non coulé), la partie continue
                    if (cellule != "~" && cellule != "X" && cellule != "O")
                    {
                        return false;
                    }
                }
            }

            return true; // Tous les bateaux sont coulés
        }

        // Fonction pour remplir la grille avec le symbole ~
        static void RemplirGrille(string[,] grille)
        {
            int hauteur = grille.GetLength(0);
            int largeur = grille.GetLength(1);

            for (int iLigne = 0; iLigne < hauteur; iLigne++)
            {
                for (int iCol = 0; iCol < largeur; iCol++)
                {
                    grille[iLigne, iCol] = "~";
                }
            }
        }

        static void AfficherGrille(int largeur, int hauteur, string[,] grille)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string coinSupGauche = "┌";
            string coinSupDroit = "┐";
            string coinInfGauche = "└";
            string coinInfDroit = "┘";
            string horizontal = "─";
            string vertical = "│";
            string croixHaut = "┬";
            string croixBas = "┴";
            string croixGauche = "├";
            string croixDroite = "┤";
            string croixCentre = "┼";
            int largeurCellule = 3;
            //Affichage des lettres au dessus des colonnes
            Console.Write("   "); // espace pour les numéros de ligne
            for (int col = 0; col < largeur; col++)
            {
                char lettre = (char)('A' + col);
                Console.Write(" " + lettre + " ".PadLeft(largeurCellule - 1));
            }
            Console.WriteLine();
            //ligne du haut
            Console.Write("   "); //aligner avec les coordonnées
            Console.Write(coinSupGauche);
            for (int col = 0; col < largeur; col++)
            {
                for (int i = 0; i < largeurCellule; i++)
                    Console.Write(horizontal);
                if (col < largeur - 1)
                    Console.Write(croixHaut);
            }
            Console.WriteLine(coinSupDroit);
            //corps de la grille
            for (int ligne = 0; ligne < hauteur; ligne++)
            {
                // Ligne avec le contenu des cellules
                Console.Write((ligne + 1).ToString().PadLeft(2) + " "); //coordonnée Y à gauche
                Console.Write(vertical);
                for (int col = 0; col < largeur; col++)
                {
                    string contenu = grille[ligne, col];
                    // Centrer le contenu dans la cellule
                    int espacesAvant = (largeurCellule - contenu.Length) / 2;
                    int espacesApres = largeurCellule - contenu.Length - espacesAvant;

                    Console.Write(new string(' ', espacesAvant));
                    Console.Write(contenu);
                    Console.Write(new string(' ', espacesApres));
                    Console.Write(vertical);
                }
                Console.WriteLine();
                //Ligne de séparation horizontale (sauf dernière)
                if (ligne < hauteur - 1)
                {
                    Console.Write("   "); //alignement avec les coordonnées
                    Console.Write(croixGauche);
                    for (int col = 0; col < largeur; col++)
                    {
                        for (int i = 0; i < largeurCellule; i++)
                            Console.Write(horizontal);
                        if (col < largeur - 1)
                            Console.Write(croixCentre);
                    }
                    Console.WriteLine(croixDroite);
                }
            }
            //ligne du bas
            Console.Write("   "); //alignement avec les coordonnées
            Console.Write(coinInfGauche);
            for (int col = 0; col < largeur; col++)
            {
                for (int i = 0; i < largeurCellule; i++)
                    Console.Write(horizontal);
                if (col < largeur - 1)
                    Console.Write(croixBas);
            }
            Console.WriteLine(coinInfDroit);
        }

        // Types de bateaux disponibles
        enum TypeBateau
        {
            PorteAvions = 5,    // 5 cases
            Cuirasse = 4,       // 4 cases
            Croiseur = 3,       // 3 cases
            SousMarin = 2,      // 2 cases
            Destroyer = 1       // 1 cases
        }

        // Fonction pour placer un bateau sur la grille
        static bool PlacerBateau(string[,] grille, string coordonnee, TypeBateau typeBateau, bool horizontal)
        {
            // Convertir la coordonnée (ex: "A1") en indices de tableau
            if (coordonnee.Length < 2)
            {
                Console.WriteLine("Coordonnée invalide !");
                return false;
            }

            char lettreCol = char.ToUpper(coordonnee[0]);
            string numLigne = coordonnee.Substring(1);

            // Vérifier que la lettre est valide (A-J)
            if (lettreCol < 'A' || lettreCol > 'J')
            {
                Console.WriteLine("La colonne doit être entre A et J !");
                return false;
            }

            // Convertir la colonne (A=0, B=1, etc.)
            int colonne = lettreCol - 'A';

            // Convertir le numéro de ligne
            if (!int.TryParse(numLigne, out int ligne) || ligne < 1 || ligne > 10)
            {
                Console.WriteLine("La ligne doit être entre 1 et 10 !");
                return false;
            }
            ligne--; // Ajuster pour l'index du tableau (0-9)

            int tailleBateau = (int)typeBateau;
            int hauteur = grille.GetLength(0);
            int largeur = grille.GetLength(1);

            // Vérifier si le bateau rentre dans la grille
            if (horizontal)
            {
                if (colonne + tailleBateau > largeur)
                {
                    Console.WriteLine("Le bateau dépasse de la grille horizontalement !");
                    return false;
                }
            }
            else // vertical
            {
                if (ligne + tailleBateau > hauteur)
                {
                    Console.WriteLine("Le bateau dépasse de la grille verticalement !");
                    return false;
                }
            }

            // Vérifier si les cases sont libres
            for (int i = 0; i < tailleBateau; i++)
            {
                int checkLigne = horizontal ? ligne : ligne + i;
                int checkCol = horizontal ? colonne + i : colonne;

                if (grille[checkLigne, checkCol] != "~")
                {
                    Console.WriteLine("Une case est déjà occupée !");
                    return false;
                }
            }

            // Placer le bateau
            char symboleBateau = ObtenirSymboleBateau(typeBateau);
            for (int i = 0; i < tailleBateau; i++)
            {
                int placeLigne = horizontal ? ligne : ligne + i;
                int placeCol = horizontal ? colonne + i : colonne;
                grille[placeLigne, placeCol] = symboleBateau.ToString();
            }

            Console.WriteLine($"{typeBateau} placé avec succès !");
            return true;
        }

        // Fonction pour obtenir le symbole du bateau 
        static char ObtenirSymboleBateau(TypeBateau type)
        {
            switch (type)
            {
                case TypeBateau.PorteAvions:
                    return 'P';
                case TypeBateau.Cuirasse:
                    return 'C';
                case TypeBateau.Croiseur:
                    return 'R';
                case TypeBateau.SousMarin:
                    return 'S';
                case TypeBateau.Destroyer:
                    return 'D';
                default:
                    return 'B';
            }
        }

        // Fonction interactive pour placer tous les bateaux d'un joueur
        static void PlacerTousLesBateaux(string[,] grille, string nomJoueur)
        {
            Console.WriteLine($"\n=== Placement des bateaux pour {nomJoueur} ===\n");

            // Tableau pour suivre quels bateaux ont été placés
            bool[] bateauxPlaces = new bool[5];
            TypeBateau[] typesBateaux = {
            TypeBateau.PorteAvions,
            TypeBateau.Cuirasse,
            TypeBateau.Croiseur,
            TypeBateau.SousMarin,
            TypeBateau.Destroyer};

            int bateauxRestants = 5;

            while (bateauxRestants > 0)
            {
                Console.WriteLine("\nBateaux disponibles :");
                for (int i = 0; i < typesBateaux.Length; i++)
                {
                    if (!bateauxPlaces[i])
                    {
                        Console.WriteLine($"{i + 1}. {typesBateaux[i]} ({(int)typesBateaux[i]} cases) - Symbole: {ObtenirSymboleBateau(typesBateaux[i])}");
                    }
                }

                Console.Write("\nChoisissez un bateau à placer (1-5) : ");
                if (!int.TryParse(Console.ReadLine(), out int choix) || choix < 1 || choix > 5)
                {
                    Console.WriteLine("Choix invalide !");
                    continue;
                }

                int index = choix - 1;
                if (bateauxPlaces[index])
                {
                    Console.WriteLine("Ce bateau a déjà été placé !");
                    continue;
                }

                TypeBateau bateauChoisi = typesBateaux[index];

                Console.Write("Coordonnée de départ (ex: A1) : ");
                string coordonnee = Console.ReadLine().Trim();

                Console.Write("Orientation (H pour horizontal, V pour vertical) : ");
                string orientation = Console.ReadLine().Trim().ToUpper();
                bool horizontal = orientation == "H";

                if (PlacerBateau(grille, coordonnee, bateauChoisi, horizontal))
                {
                    bateauxPlaces[index] = true;
                    bateauxRestants--;

                    Console.WriteLine("\nGrille actuelle :");
                    AfficherGrille(10, 10, grille);
                }
                else
                {
                    Console.WriteLine("Placement échoué, réessayez !");
                }
            }

            Console.WriteLine($"\n{nomJoueur} a placé tous ses bateaux !");
        }

        // Fonction pour afficher les informations sur les bateaux
        static void AfficherInfosBateaux()
        {
            Console.WriteLine("\n=== Informations sur les bateaux ===");
            Console.WriteLine("1. Porte-avions (P) : 5 cases");
            Console.WriteLine("2. Cuirassé (C)     : 4 cases");
            Console.WriteLine("3. Croiseur (R)     : 3 cases");
            Console.WriteLine("4. Sous-marin (S)   : 2 cases");
            Console.WriteLine("5. Destroyer (D)    : 1 case");
            Console.WriteLine("\nChaque joueur dispose d'un exemplaire de chaque bateau.");
            Console.WriteLine("Les bateaux peuvent être placés horizontalement (H) ou verticalement (V).\n");
        }
    }
}
