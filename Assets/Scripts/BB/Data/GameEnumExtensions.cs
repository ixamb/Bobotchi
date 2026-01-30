using System;

namespace BB.Data
{
    public static class FurnitureCollectionExtensions
    {
        public static string ToTranslatedString(this Gender gender)
        {
            return gender switch
            {
                Gender.Male => "Homme",
                Gender.Female => "Female",
                Gender.NonBinary => "Non binaire",
                _ => gender.ToString()
            };
        }
        
        public static string ToTranslatedString(this FurnitureCollection furnitureCollection)
        {
            return furnitureCollection switch
            {
                FurnitureCollection.Basics => "Les basiques",
                FurnitureCollection.Pop => "Vent pop",
                FurnitureCollection.Seventies => "Eighties - 10",
                FurnitureCollection.Cottage => "Cozy cottage",
                FurnitureCollection.Minimalist => "Millenial beige",
                FurnitureCollection.London => "London calling",
                FurnitureCollection.Surf => "Surfer du dimanche",
                _ => furnitureCollection.ToString()
            };
        }

        public static string ToTranslatedString(this PropCategory propCategory)
        {
            return propCategory switch
            {
                PropCategory.OnGround => "Mobilier",
                PropCategory.OnWall => "Pour le mur",
                PropCategory.Walkable => "Au sol",
                _ => propCategory.ToString()
            };
        }

        public static string ToTranslatedString(this SurfaceType surfaceType)
        {
            return surfaceType switch
            {
                SurfaceType.Wall => "Papier peint",
                SurfaceType.Floor => "Sol",
                _ => surfaceType.ToString()
            };
        }

        public static string ToTranslatedString(this CharacterStateStat state)
        {
            return state switch
            {
                CharacterStateStat.Energy => "Énergie",
                CharacterStateStat.Hunger => "Faim",
                CharacterStateStat.Esteem => "Self-estime",
                _ => state.ToString()
            };
        }
    }
}