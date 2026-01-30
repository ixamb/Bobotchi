#!/usr/bin/env python3
"""
Script pour cropper automatiquement les images avec fond transparent.
Supprime tous les bords transparents pour ne garder que le contenu visible.
"""

from PIL import Image
import os
import sys
from pathlib import Path

def find_content_bounds(image):
    """
    Trouve les limites du contenu non-transparent dans l'image.
    Retourne un tuple (left, top, right, bottom) ou None si l'image est vide.
    """
    # Obtenir les données alpha (canal de transparence)
    if image.mode != 'RGBA':
        # Convertir en RGBA si nécessaire
        image = image.convert('RGBA')
    
    # Récupérer le bbox du contenu non-transparent
    # getbbox() retourne (left, top, right, bottom) de la zone non-transparente
    bbox = image.getbbox()
    
    return bbox

def crop_image(input_path, output_path=None, suffix="_cropped", overwrite=False):
    """
    Crop une image en supprimant les bords transparents.
    
    Args:
        input_path: Chemin de l'image source
        output_path: Chemin de sauvegarde (optionnel)
        suffix: Suffixe à ajouter au nom de fichier si output_path n'est pas fourni
        overwrite: Si True, écrase l'image originale
    
    Returns:
        Le chemin de l'image croppée
    """
    try:
        # Ouvrir l'image
        img = Image.open(input_path)
        
        # Trouver les limites du contenu
        bbox = find_content_bounds(img)
        
        if bbox is None:
            print(f"⚠️  '{input_path}' est complètement transparente, ignorée.")
            return None
        
        # Dimensions originales
        original_size = img.size
        
        # Cropper l'image
        cropped_img = img.crop(bbox)
        cropped_size = cropped_img.size
        
        # Déterminer le chemin de sauvegarde
        if overwrite:
            save_path = input_path
        elif output_path:
            save_path = output_path
        else:
            # Générer un nom avec suffixe
            path = Path(input_path)
            save_path = path.parent / f"{path.stem}{suffix}{path.suffix}"
        
        # Sauvegarder
        cropped_img.save(save_path, 'PNG')
        
        # Afficher les infos
        reduction = (1 - (cropped_size[0] * cropped_size[1]) / (original_size[0] * original_size[1])) * 100
        print(f"✅ '{Path(input_path).name}': {original_size[0]}x{original_size[1]} → {cropped_size[0]}x{cropped_size[1]} (-{reduction:.1f}%)")
        
        return save_path
        
    except Exception as e:
        print(f"❌ Erreur avec '{input_path}': {e}")
        return None

def crop_directory(directory, recursive=False, output_dir=None, overwrite=False, extensions=None):
    """
    Crop toutes les images d'un dossier.
    
    Args:
        directory: Dossier source
        recursive: Si True, parcourt les sous-dossiers
        output_dir: Dossier de destination (optionnel)
        overwrite: Si True, écrase les images originales
        extensions: Liste des extensions à traiter (par défaut: png)
    """
    if extensions is None:
        extensions = ['.png']
    
    directory = Path(directory)
    
    if not directory.exists():
        print(f"❌ Le dossier '{directory}' n'existe pas!")
        return
    
    # Créer le dossier de sortie si nécessaire
    if output_dir and not overwrite:
        output_dir = Path(output_dir)
        output_dir.mkdir(parents=True, exist_ok=True)
    
    # Trouver tous les fichiers
    pattern = "**/*" if recursive else "*"
    files = []
    for ext in extensions:
        files.extend(directory.glob(f"{pattern}{ext}"))
    
    if not files:
        print(f"⚠️  Aucune image trouvée dans '{directory}'")
        return
    
    print(f"\n🔍 {len(files)} image(s) trouvée(s)\n")
    
    processed = 0
    for file_path in files:
        # Déterminer le chemin de sortie
        if output_dir and not overwrite:
            relative_path = file_path.relative_to(directory)
            out_path = output_dir / relative_path
            out_path.parent.mkdir(parents=True, exist_ok=True)
        else:
            out_path = None
        
        result = crop_image(file_path, out_path, overwrite=overwrite)
        if result:
            processed += 1
    
    print(f"\n✨ Terminé! {processed}/{len(files)} image(s) traitée(s)")

def main():
    import argparse
    
    parser = argparse.ArgumentParser(
        description='Crop automatiquement les images avec fond transparent',
        formatter_class=argparse.RawDescriptionHelpFormatter,
        epilog="""
Exemples:
  # Cropper une image dans le dossier courant
  python autocrop.py sprite.png
  
  # Cropper et écraser l'original
  python autocrop.py sprite.png --overwrite
  
  # Cropper TOUTES les images du dossier courant (écrase les originaux)
  python autocrop.py --all
  
  # Cropper toutes les images avec suffixe _cropped (sans écraser)
  python autocrop.py --all --no-overwrite
  
  # Cropper une image avec un chemin complet
  python autocrop.py Assets/Sprites/sprite.png
  
  # Cropper toutes les images d'un dossier
  python autocrop.py mon_dossier/
  
  # Cropper récursivement
  python autocrop.py source/ -r --overwrite
        """
    )
    
    parser.add_argument('filename', nargs='?', help='Nom du fichier (cherche dans le dossier courant) ou chemin complet vers un fichier/dossier')
    parser.add_argument('-o', '--output', help='Chemin de sortie pour le fichier croppé')
    parser.add_argument('-r', '--recursive', action='store_true', help='Parcourir les sous-dossiers (mode dossier uniquement)')
    parser.add_argument('--overwrite', action='store_true', default=None, help='Écraser les images originales')
    parser.add_argument('--no-overwrite', action='store_true', help='Ne pas écraser (crée des copies avec suffixe _cropped)')
    parser.add_argument('--all', action='store_true', help='Traiter toutes les images du dossier courant')
    parser.add_argument('-e', '--extensions', nargs='+', default=['.png'], 
                        help='Extensions à traiter en mode dossier (défaut: .png)')
    
    args = parser.parse_args()
    
    # Déterminer le mode overwrite
    if args.no_overwrite:
        overwrite = False
    elif args.overwrite is not None:
        overwrite = args.overwrite
    else:
        # Par défaut : overwrite en mode --all, pas overwrite sinon
        overwrite = args.all
    
    # Mode --all : traiter toutes les images du dossier courant
    if args.all:
        current_dir = Path.cwd()
        print(f"🔍 Mode --all : traitement de toutes les images dans '{current_dir}'")
        if overwrite:
            print("⚠️  Les images originales seront ÉCRASÉES\n")
        else:
            print("ℹ️  Les images croppées auront le suffixe '_cropped'\n")
        
        crop_directory(current_dir, args.recursive, args.output, overwrite, args.extensions)
        return
    
    # Si aucun fichier spécifié, erreur
    if not args.filename:
        parser.print_help()
        print("\n❌ Erreur: Vous devez spécifier un fichier/dossier ou utiliser --all")
        sys.exit(1)
    
    # Construire le chemin
    input_path = Path(args.filename)
    
    # Si c'est juste un nom de fichier (pas de slash), chercher dans le dossier courant
    if not input_path.is_absolute() and '/' not in str(input_path) and '\\' not in str(input_path):
        # C'est juste un nom de fichier, chercher dans le dossier courant
        input_path = Path.cwd() / input_path
    
    # Vérifier que le chemin existe
    if not input_path.exists():
        print(f"❌ '{input_path}' n'existe pas!")
        sys.exit(1)
    
    if input_path.is_file():
        # Cropper une seule image
        crop_image(input_path, args.output, overwrite=overwrite)
    elif input_path.is_dir():
        # Cropper un dossier
        crop_directory(input_path, args.recursive, args.output, overwrite, args.extensions)
    else:
        print(f"❌ '{input_path}' n'est ni un fichier ni un dossier valide!")
        sys.exit(1)

if __name__ == '__main__':
    main()