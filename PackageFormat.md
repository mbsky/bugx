#Package Format

Voici le format des paquages d'installation, tel que défini pour le moment:
Il contient:
  1. Un fichier manifest.xml.

Outre le nom et la description du paquet, ce fichier comporte
  * le numéro de version. Ce dernier est codé sur 3 chiffres séparés par des points. Le premier chiffre correspond à un numéro de version majeur du module, et est incrémenté manuellement ou automatiquement (à voir) quand on modifie le comportement d'un module en terme de fonctionnalités. Le 2eme correspond au numéro de version mineure, et est incrémenté manuellement ou automatiquement en cas de debug susceptible d'entrainer une incompatibilité, ou de debug important (changement de taille ou ajout d'un champ dans la db, ajout d'un système de cache car problèmes de perfs, ...). Le 3eme est incrémenter de façon automatique à chaque fois que l'on créé un paquet d'installation. Théoriquement, le fait d'incrémenter ce chiffre correspond à un debug mineur (ajout d'un test sur une valeur non nulle, ce genre de chose). Je pense aussi qu'il serait bon de remettre systématiquement ce chiffre à 0 ou 1 quand on touche à l'un des 2 autres. (rmq: on fera de même avec le 2eme quand on touche au 1er).

  * Une liste des assembly à installer, avec pour chacune une liste des dépendances.

  * La liste des fichiers sql à exécuter lors de l'install. Ce sont principalement les procédures stockées.

  1. 3 repertoires
  * bin: contient les assembly
  * sql: contient les fichiers sql présents dans le fichier manifest.xml + custom.install.sql (commandes sql spécifiques à exécuter lors de l'install) + custom.uninstall.sql (commandes sql spécifiques à exécuter quand on désinstalle le paquet).
  * files: autres fichiers à installer. L'arborescence des repertoires files et bin correspond à l'endroit ou installer les fichiers. Ainsi, les fichiers se trouvant dans files/wvnportal/ressources seront à copier dans le repertoire correspond sur le serveur. Dans cette arborescence, les noms entourés de "%" seront substitués par leur équivalent. exemple: %desktopmodules% sera remplacé par wvnportal/desktopmodules.

  1. un fichier web.config.
Ce dernier reprend exactement la même structure que le fichier web.config global et reprend toutes les clés utilisées par le module.