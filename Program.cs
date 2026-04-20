namespace Tirer_Joueur
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Message de bienvenue
            Console.WriteLine("Bienvenu dans mon programme !");

            // ===== DÉCLARATION ET INITIALISATION DES VARIABLES =====
            int largeur; // Nombre de colonnes de la grille (A-J = 10)
            int hauteur; // Nombre de lignes de la grille (1-10 = 10)
            largeur = 10;
            hauteur = 10;

            // ===== CRÉATION DES GRILLES DE JEU =====
            // Grilles contenant les bateaux de chaque joueur
            string[,] grilleJoueur1 = new string[hauteur, largeur]; // Grille des bateaux du joueur 1
            string[,] grilleJoueur2 = new string[hauteur, largeur]; // Grille des bateaux du joueur 2

            // Grilles pour tracer les tirs de chaque joueur (ce qu'ils voient de l'adversaire)
            string[,] grilleTirsJoueur1 = new string[hauteur, largeur]; // Joueur 1 tire sur joueur 2
            string[,] grilleTirsJoueur2 = new string[hauteur, largeur]; // Joueur 2 tire sur joueur 1

            // ===== PHASE DE PLACEMENT - JOUEUR 1 =====
            RemplirGrille(grilleJoueur1);        // Remplir la grille de bateaux avec des vagues (~)
            RemplirGrille(grilleTirsJoueur1);    // Remplir la grille de tirs avec des vagues (~)
            AfficherInfosBateaux();               // Afficher les types de bateaux disponibles
            PlacerTousLesBateaux(grilleJoueur1, "JOUEUR 1"); // Le joueur 1 place ses 5 bateaux

            // ===== TRANSITION ENTRE LES JOUEURS =====
            Console.WriteLine("\n\n=== Le Joueur 1 a terminé ! ===");
            Console.WriteLine("Appuyez sur une touche pour que le Joueur 2 commence...");
            Console.ReadKey();   // Attendre que l'utilisateur appuie sur une touche
            Console.Clear();     // Effacer l'écran pour que le joueur 2 ne voie pas les bateaux du joueur 1

            // ===== PHASE DE PLACEMENT - JOUEUR 2 =====
            RemplirGrille(grilleJoueur2);        // Remplir la grille de bateaux avec des vagues (~)
            RemplirGrille(grilleTirsJoueur2);    // Remplir la grille de tirs avec des vagues (~)
            AfficherInfosBateaux();               // Afficher les types de bateaux disponibles
            PlacerTousLesBateaux(grilleJoueur2, "JOUEUR 2"); // Le joueur 2 place ses 5 bateaux

            // ===== DÉBUT DE LA BATAILLE =====
            Console.Clear();     // Effacer l'écran
            Console.WriteLine("=== Que la bataille commence ! ===\n");
            Console.WriteLine("Appuyez sur une touche pour commencer...");
            Console.ReadKey();   // Attendre avant de commencer

            // Lancer la boucle de jeu (tirs alternés jusqu'à ce qu'un joueur gagne)
            JouerBataille(grilleJoueur1, grilleJoueur2, grilleTirsJoueur1, grilleTirsJoueur2, largeur, hauteur);
        }

        // ===== FONCTION PRINCIPALE DE LA BATAILLE =====
        // Cette fonction gère toute la partie de jeu (tirs alternés)
        static void JouerBataille(string[,] grilleJ1, string[,] grilleJ2, string[,] tirsJ1, string[,] tirsJ2, int largeur, int hauteur)
        {
            bool joueur1Actif = true;  // true = c'est le tour du joueur 1, false = tour du joueur 2
            bool partieEnCours = true; // true = la partie continue, false = un joueur a gagné

            // Boucle principale du jeu - continue tant que personne n'a gagné
            while (partieEnCours)
            {
                Console.Clear(); // Effacer l'écran à chaque tour

                // ===== TOUR DU JOUEUR 1 =====
                if (joueur1Actif)
                {
                    Console.WriteLine("=== Tour du JOUEUR 1 ===\n");
                    Console.WriteLine("Vos tirs précédents :");
                    AfficherGrille(largeur, hauteur, tirsJ1); // Afficher les tirs déjà effectués

                    // Le joueur 1 effectue un tir sur la grille du joueur 2
                    Tirer(grilleJ2, tirsJ1, "JOUEUR 1");

                    // Vérifier si tous les bateaux du joueur 2 sont coulés
                    if (TousLesBateauxCoules(grilleJ2))
                    {
                        Console.WriteLine("\n🎉🎉🎉 JOUEUR 1 A GAGNÉ ! 🎉🎉🎉");
                        partieEnCours = false; // Arrêter la partie
                    }
                }
                // ===== TOUR DU JOUEUR 2 =====
                else
                {
                    Console.WriteLine("=== Tour du JOUEUR 2 ===\n");
                    Console.WriteLine("Vos tirs précédents :");
                    AfficherGrille(largeur, hauteur, tirsJ2); // Afficher les tirs déjà effectués

                    // Le joueur 2 effectue un tir sur la grille du joueur 1
                    Tirer(grilleJ1, tirsJ2, "JOUEUR 2");

                    // Vérifier si tous les bateaux du joueur 1 sont coulés
                    if (TousLesBateauxCoules(grilleJ1))
                    {
                        Console.WriteLine("\n🎉🎉🎉 JOUEUR 2 A GAGNÉ ! 🎉🎉🎉");
                        partieEnCours = false; // Arrêter la partie
                    }
                }

                // Si la partie continue, passer au joueur suivant
                if (partieEnCours)
                {
                    Console.WriteLine("\nAppuyez sur une touche pour passer au joueur suivant...");
                    Console.ReadKey();
                    joueur1Actif = !joueur1Actif; // Inverser le joueur actif (! = opérateur NOT)
                }
            }

            // Fin de partie
            Console.WriteLine("\nAppuyez sur une touche pour quitter...");
            Console.ReadKey();
        }

        // ===== FONCTION POUR EFFECTUER UN TIR =====
        static void Tirer(string[,] grilleAdversaire, string[,] grilleTirs, string nomJoueur)
        {
            bool tirValide = false; // Devient true quand le tir est valide

            // Boucle jusqu'à ce que le joueur entre des coordonnées valides
            while (!tirValide)
            {
                Console.Write("\nEntrez les coordonnées de tir (ex: A1) : ");
                string coordonnee = Console.ReadLine().Trim(); // Lire et supprimer les espaces

                // ===== VALIDATION DE LA COORDONNÉE =====
                // Vérifier que la coordonnée a au moins 2 caractères (ex: A1)
                if (coordonnee.Length < 2)
                {
                    Console.WriteLine("Coordonnée invalide !");
                    continue; // Recommencer la boucle
                }

                char lettreCol = char.ToUpper(coordonnee[0]);    // Prendre la première lettre et la mettre en majuscule
                string numLigne = coordonnee.Substring(1);        // Prendre tout ce qui suit la première lettre

                // Vérifier que la lettre est entre A et J
                if (lettreCol < 'A' || lettreCol > 'J')
                {
                    Console.WriteLine("La colonne doit être entre A et J !");
                    continue; // Recommencer la boucle
                }

                // Convertir la lettre en numéro de colonne (A=0, B=1, C=2, etc.)
                int colonne = lettreCol - 'A';

                // Convertir le numéro de ligne en entier et vérifier qu'il est entre 1 et 10
                if (!int.TryParse(numLigne, out int ligne) || ligne < 1 || ligne > 10)
                {
                    Console.WriteLine("La ligne doit être entre 1 et 10 !");
                    continue; // Recommencer la boucle
                }
                ligne--; // Ajuster pour l'index du tableau (l'utilisateur entre 1-10, mais le tableau va de 0-9)

                // Vérifier si on a déjà tiré à cet endroit
                if (grilleTirs[ligne, colonne] != "~")
                {
                    Console.WriteLine("Vous avez déjà tiré à cet endroit !");
                    continue; // Recommencer la boucle
                }

                // ===== TIR VALIDE - VÉRIFIER LE RÉSULTAT =====
                tirValide = true; // Le tir est valide, on sort de la boucle

                // Récupérer ce qui se trouve à cette position sur la grille adverse
                string caseAdversaire = grilleAdversaire[ligne, colonne];

                // ===== CAS 1 : RATÉ (eau) =====
                if (caseAdversaire == "~")
                {
                    Console.WriteLine("\n💧 MANQUÉ !");
                    grilleTirs[ligne, colonne] = "O";        // Marquer "O" sur la grille de tirs
                    grilleAdversaire[ligne, colonne] = "O";  // Marquer "O" sur la grille adverse
                }
                // ===== CAS 2 : TOUCHÉ (bateau) =====
                else
                {
                    char bateauTouche = caseAdversaire[0];   // Récupérer le symbole du bateau (P, C, R, S, ou D)
                    grilleTirs[ligne, colonne] = "X";        // Marquer "X" sur la grille de tirs
                    grilleAdversaire[ligne, colonne] = "X";  // Marquer "X" sur la grille adverse

                    // Vérifier si le bateau est complètement coulé
                    if (BateauCoule(grilleAdversaire, bateauTouche))
                    {
                        string nomBateau = ObtenirNomBateau(bateauTouche); // Obtenir le nom complet du bateau
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

        // ===== FONCTION POUR VÉRIFIER SI UN BATEAU EST COMPLÈTEMENT COULÉ =====
        static bool BateauCoule(string[,] grille, char symboleBateau)
        {
            int hauteur = grille.GetLength(0); // Obtenir le nombre de lignes
            int largeur = grille.GetLength(1); // Obtenir le nombre de colonnes

            // Parcourir toute la grille
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    // Si on trouve encore le symbole du bateau (pas un X), le bateau n'est pas coulé
                    if (grille[i, j].Length > 0 && grille[i, j][0] == symboleBateau)
                    {
                        return false; // Le bateau a encore des parties intactes
                    }
                }
            }

            return true; // On n'a trouvé aucune partie intacte, le bateau est coulé
        }

        // ===== FONCTION POUR OBTENIR LE NOM COMPLET D'UN BATEAU =====
        static string ObtenirNomBateau(char symbole)
        {
            // Convertir le symbole (P, C, R, S, D) en nom complet
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

        // ===== FONCTION POUR VÉRIFIER SI TOUS LES BATEAUX SONT COULÉS =====
        static bool TousLesBateauxCoules(string[,] grille)
        {
            int hauteur = grille.GetLength(0); // Obtenir le nombre de lignes
            int largeur = grille.GetLength(1); // Obtenir le nombre de colonnes

            // Parcourir toute la grille
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    string cellule = grille[i, j];
                    // Si on trouve une lettre (P, C, R, S, D), il reste un bateau non coulé
                    if (cellule != "~" && cellule != "X" && cellule != "O")
                    {
                        return false; // Il reste au moins un bateau
                    }
                }
            }

            return true; // Tous les bateaux sont coulés (que des ~, X et O)
        }

        // ===== FONCTION POUR REMPLIR UNE GRILLE AVEC DES VAGUES =====
        static void RemplirGrille(string[,] grille)
        {
            int hauteur = grille.GetLength(0); // Obtenir le nombre de lignes
            int largeur = grille.GetLength(1); // Obtenir le nombre de colonnes

            // Parcourir toutes les cases de la grille
            for (int iLigne = 0; iLigne < hauteur; iLigne++)
            {
                for (int iCol = 0; iCol < largeur; iCol++)
                {
                    grille[iLigne, iCol] = "~"; // Mettre une vague dans chaque case
                }
            }
        }

        // ===== FONCTION POUR AFFICHER UNE GRILLE =====
        static void AfficherGrille(int largeur, int hauteur, string[,] grille)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // Permettre l'affichage de caractères spéciaux

            // Caractères pour dessiner les bordures de la grille
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
            int largeurCellule = 3; // Chaque cellule fait 3 caractères de large

            // ===== AFFICHAGE DES LETTRES DE COLONNES (A B C D E F G H I J) =====
            Console.Write("   "); // Espace pour aligner avec les numéros de ligne
            for (int col = 0; col < largeur; col++)
            {
                char lettre = (char)('A' + col); // Convertir 0→A, 1→B, 2→C, etc.
                Console.Write(" " + lettre + " ".PadLeft(largeurCellule - 1)); // Afficher la lettre centrée
            }
            Console.WriteLine(); // Retour à la ligne

            // ===== LIGNE DU HAUT DE LA GRILLE (┌───┬───┬───┐) =====
            Console.Write("   "); // Alignement
            Console.Write(coinSupGauche); // Coin supérieur gauche
            for (int col = 0; col < largeur; col++)
            {
                for (int i = 0; i < largeurCellule; i++)
                    Console.Write(horizontal); // Ligne horizontale
                if (col < largeur - 1)
                    Console.Write(croixHaut); // Croix en haut entre les colonnes
            }
            Console.WriteLine(coinSupDroit); // Coin supérieur droit

            // ===== CORPS DE LA GRILLE (lignes avec contenu) =====
            for (int ligne = 0; ligne < hauteur; ligne++)
            {
                // ===== AFFICHAGE DU NUMÉRO DE LIGNE ET DU CONTENU =====
                Console.Write((ligne + 1).ToString().PadLeft(2) + " "); // Numéro de ligne (1-10)
                Console.Write(vertical); // Bordure gauche

                // Afficher chaque cellule de la ligne
                for (int col = 0; col < largeur; col++)
                {
                    string contenu = grille[ligne, col]; // Récupérer le contenu (~, X, O, P, C, etc.)

                    // Centrer le contenu dans la cellule
                    int espacesAvant = (largeurCellule - contenu.Length) / 2;
                    int espacesApres = largeurCellule - contenu.Length - espacesAvant;

                    Console.Write(new string(' ', espacesAvant)); // Espaces avant
                    Console.Write(contenu);                        // Contenu
                    Console.Write(new string(' ', espacesApres));  // Espaces après
                    Console.Write(vertical);                       // Bordure entre cellules
                }
                Console.WriteLine(); // Retour à la ligne

                // ===== LIGNE DE SÉPARATION HORIZONTALE (├───┼───┼───┤) =====
                if (ligne < hauteur - 1) // Pas de ligne après la dernière ligne
                {
                    Console.Write("   "); // Alignement
                    Console.Write(croixGauche); // Croix à gauche
                    for (int col = 0; col < largeur; col++)
                    {
                        for (int i = 0; i < largeurCellule; i++)
                            Console.Write(horizontal); // Ligne horizontale
                        if (col < largeur - 1)
                            Console.Write(croixCentre); // Croix au centre
                    }
                    Console.WriteLine(croixDroite); // Croix à droite
                }
            }

            // ===== LIGNE DU BAS DE LA GRILLE (└───┴───┴───┘) =====
            Console.Write("   "); // Alignement
            Console.Write(coinInfGauche); // Coin inférieur gauche
            for (int col = 0; col < largeur; col++)
            {
                for (int i = 0; i < largeurCellule; i++)
                    Console.Write(horizontal); // Ligne horizontale
                if (col < largeur - 1)
                    Console.Write(croixBas); // Croix en bas
            }
            Console.WriteLine(coinInfDroit); // Coin inférieur droit
        }

        // ===== ÉNUMÉRATION DES TYPES DE BATEAUX =====
        // Les valeurs (5, 4, 3, 2, 1) représentent le nombre de cases de chaque bateau
        enum TypeBateau
        {
            PorteAvions = 5,    // 5 cases
            Cuirasse = 4,       // 4 cases
            Croiseur = 3,       // 3 cases
            SousMarin = 2,      // 2 cases
            Destroyer = 1       // 1 case
        }

        // ===== FONCTION POUR PLACER UN BATEAU SUR LA GRILLE =====
        static bool PlacerBateau(string[,] grille, string coordonnee, TypeBateau typeBateau, bool horizontal)
        {
            // ===== VALIDATION DE LA COORDONNÉE =====
            if (coordonnee.Length < 2)
            {
                Console.WriteLine("Coordonnée invalide !");
                return false; // Échec du placement
            }

            char lettreCol = char.ToUpper(coordonnee[0]);  // Première lettre en majuscule
            string numLigne = coordonnee.Substring(1);      // Reste de la chaîne (le numéro)

            // Vérifier que la lettre est entre A et J
            if (lettreCol < 'A' || lettreCol > 'J')
            {
                Console.WriteLine("La colonne doit être entre A et J !");
                return false;
            }

            // Convertir la lettre en index de colonne (A=0, B=1, etc.)
            int colonne = lettreCol - 'A';

            // Convertir le numéro de ligne en entier
            if (!int.TryParse(numLigne, out int ligne) || ligne < 1 || ligne > 10)
            {
                Console.WriteLine("La ligne doit être entre 1 et 10 !");
                return false;
            }
            ligne--; // Ajuster pour l'index du tableau (1-10 devient 0-9)

            int tailleBateau = (int)typeBateau; // Obtenir la taille du bateau
            int hauteur = grille.GetLength(0);   // Hauteur de la grille
            int largeur = grille.GetLength(1);   // Largeur de la grille

            // ===== VÉRIFIER SI LE BATEAU RENTRE DANS LA GRILLE =====
            if (horizontal)
            {
                // Pour un bateau horizontal, vérifier qu'il ne dépasse pas à droite
                if (colonne + tailleBateau > largeur)
                {
                    Console.WriteLine("Le bateau dépasse de la grille horizontalement !");
                    return false;
                }
            }
            else // Vertical
            {
                // Pour un bateau vertical, vérifier qu'il ne dépasse pas en bas
                if (ligne + tailleBateau > hauteur)
                {
                    Console.WriteLine("Le bateau dépasse de la grille verticalement !");
                    return false;
                }
            }

            // ===== VÉRIFIER SI LES CASES SONT LIBRES =====
            for (int i = 0; i < tailleBateau; i++)
            {
                // Calculer la position de chaque partie du bateau
                int checkLigne = horizontal ? ligne : ligne + i;
                int checkCol = horizontal ? colonne + i : colonne;

                // Vérifier si la case est déjà occupée
                if (grille[checkLigne, checkCol] != "~")
                {
                    Console.WriteLine("Une case est déjà occupée !");
                    return false;
                }
            }

            // ===== PLACER LE BATEAU =====
            char symboleBateau = ObtenirSymboleBateau(typeBateau); // Obtenir le symbole (P, C, R, S, D)
            for (int i = 0; i < tailleBateau; i++)
            {
                // Calculer la position de chaque partie du bateau
                int placeLigne = horizontal ? ligne : ligne + i;
                int placeCol = horizontal ? colonne + i : colonne;

                // Placer le symbole du bateau
                grille[placeLigne, placeCol] = symboleBateau.ToString();
            }

            Console.WriteLine($"{typeBateau} placé avec succès !");
            return true; // Succès du placement
        }

        // ===== FONCTION POUR OBTENIR LE SYMBOLE D'UN BATEAU =====
        static char ObtenirSymboleBateau(TypeBateau type)
        {
            // Convertir le type de bateau en symbole d'une lettre
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
                    return 'B'; // Bateau par défaut (ne devrait jamais arriver)
            }
        }

        // ===== FONCTION POUR PLACER TOUS LES BATEAUX D'UN JOUEUR =====
        static void PlacerTousLesBateaux(string[,] grille, string nomJoueur)
        {
            Console.WriteLine($"\n=== Placement des bateaux pour {nomJoueur} ===\n");

            // Tableau pour savoir quels bateaux ont déjà été placés
            bool[] bateauxPlaces = new bool[5]; // 5 bateaux, tous false au départ

            // Liste des types de bateaux disponibles
            TypeBateau[] typesBateaux = {
                TypeBateau.PorteAvions,
                TypeBateau.Cuirasse,
                TypeBateau.Croiseur,
                TypeBateau.SousMarin,
                TypeBateau.Destroyer
            };

            int bateauxRestants = 5; // Nombre de bateaux à placer

            // Boucle jusqu'à ce que tous les bateaux soient placés
            while (bateauxRestants > 0)
            {
                // ===== AFFICHER LES BATEAUX DISPONIBLES =====
                Console.WriteLine("\nBateaux disponibles :");
                for (int i = 0; i < typesBateaux.Length; i++)
                {
                    // N'afficher que les bateaux non encore placés
                    if (!bateauxPlaces[i])
                    {
                        Console.WriteLine($"{i + 1}. {typesBateaux[i]} ({(int)typesBateaux[i]} cases) - Symbole: {ObtenirSymboleBateau(typesBateaux[i])}");
                    }
                }

                // ===== DEMANDER AU JOUEUR QUEL BATEAU PLACER =====
                Console.Write("\nChoisissez un bateau à placer (1-5) : ");
                if (!int.TryParse(Console.ReadLine(), out int choix) || choix < 1 || choix > 5)
                {
                    Console.WriteLine("Choix invalide !");
                    continue; // Recommencer la boucle
                }

                int index = choix - 1; // Convertir 1-5 en 0-4 pour l'index du tableau

                // Vérifier si le bateau a déjà été placé
                if (bateauxPlaces[index])
                {
                    Console.WriteLine("Ce bateau a déjà été placé !");
                    continue; // Recommencer la boucle
                }

                TypeBateau bateauChoisi = typesBateaux[index]; // Obtenir le bateau choisi

                // ===== DEMANDER LA POSITION ET L'ORIENTATION =====
                Console.Write("Coordonnée de départ (ex: A1) : ");
                string coordonnee = Console.ReadLine().Trim(); // Lire et supprimer les espaces

                Console.Write("Orientation (H pour horizontal, V pour vertical) : ");
                string orientation = Console.ReadLine().Trim().ToUpper(); // Mettre en majuscule
                bool horizontal = orientation == "H"; // true si H, false sinon

                // ===== TENTER DE PLACER LE BATEAU =====
                if (PlacerBateau(grille, coordonnee, bateauChoisi, horizontal))
                {
                    bateauxPlaces[index] = true; // Marquer le bateau comme placé
                    bateauxRestants--;           // Décrémenter le nombre de bateaux restants

                    // Afficher la grille mise à jour
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

        // ===== FONCTION POUR AFFICHER LES INFORMATIONS SUR LES BATEAUX =====
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
