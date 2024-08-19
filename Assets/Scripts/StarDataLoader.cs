using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static StarDataLoader;

public class StarDataLoader
{
    public class Star
    {
        public float catalog_number;
        public Vector3 position;
        public Color colour;
        public float size;

        private readonly double right_ascension;
        private readonly double declination;
        private readonly float ra_proper_motion;
        private readonly float dec_proper_motion;

        public Star(float catalog_number, double right_ascension, double declination, char spectral_type,
                    byte spectral_index, short magnitude, float ra_proper_motion, float dec_proper_motion)
        {
            this.catalog_number = catalog_number;
            this.right_ascension = right_ascension;
            this.declination = declination;
            this.ra_proper_motion = ra_proper_motion;
            this.dec_proper_motion = dec_proper_motion;
            position = GetBasePosition();
            colour = SetColour(spectral_type, spectral_index);
            size = SetSize(magnitude);
        }

        public Vector3 GetBasePosition()
        {
            double x = System.Math.Cos(right_ascension);
            double y = System.Math.Sin(declination);
            double z = System.Math.Sin(right_ascension);

            double y_cos = System.Math.Cos(declination);
            x *= y_cos;
            z *= y_cos;

            return new Vector3((float)x, (float)y, (float)z);
        }

        private Color SetColour(char spectral_type, byte spectral_index)
        {
            Color IntColour(int r, int g, int b)
            {
                return new Color(r / 255f, g / 255f, b / 255f);
            }

            //Color[] col = new Color[8];
            //col[0] = IntColour(0x5c, 0x7c, 0xff); // O1
            //col[1] = IntColour(0x5d, 0x7e, 0xff); // B0.5
            //col[2] = IntColour(0x79, 0x96, 0xff); // A0
            //col[3] = IntColour(0xb8, 0xc5, 0xff); // F0
            //col[4] = IntColour(0xff, 0xef, 0xed); // G1
            //col[5] = IntColour(0xff, 0xde, 0xc0); // K0
            //col[6] = IntColour(0xff, 0xa2, 0x5a); // M0
            //col[7] = IntColour(0xff, 0x7d, 0x24); // M9.5

            // OBAFGKM colors adjusted for realism
            Color[] col = new Color[8];
            col[0] = IntColour(155, 176, 255); // O - Blue
            col[1] = IntColour(170, 191, 255); // B - Blue-White
            col[2] = IntColour(202, 215, 255); // A - White
            col[3] = IntColour(248, 247, 255); // F - Yellow-White
            col[4] = IntColour(255, 244, 234); // G - Yellow
            col[5] = IntColour(255, 210, 161); // K - Orange
            col[6] = IntColour(255, 204, 111); // M - Red-Orange
            col[7] = IntColour(255, 163, 76);  // M9.5 - Red

            int col_idx = spectral_type switch
            {
                'O' => 0,
                'B' => 1,
                'A' => 2,
                'F' => 3,
                'G' => 4,
                'K' => 5,
                'M' => 6,
                _ => -1,
            };

            if (col_idx == -1)
            {
                return Color.white;
            }

            float percent = (spectral_index - 0x30) / 10.0f;
            return Color.Lerp(col[col_idx], col[col_idx + 1], percent);
        }

        private float SetSize(short magnitude)
        {
            float size = Mathf.Pow(2.0f, (float)(-magnitude + 5) / 5.0f);
            //Debug.Log(1 - Mathf.InverseLerp(-146, 796, magnitude));
            //Debug.Log(Mathf.Pow(2.0f, (float)(-magnitude + 5) / 5.0f));
            //return size * 0.5f;
            return 1 - Mathf.InverseLerp(-146, 796, magnitude);
        }
    }

    public List<Star> LoadData()
    {
        List<Star> stars = new List<Star>();

        try
        {
            TextAsset textAsset = Resources.Load("BSC5") as TextAsset;

            if (textAsset == null)
            {
                Debug.LogError("Data file not found!");
                return stars;
            }

            using (MemoryStream stream = new MemoryStream(textAsset.bytes))
            using (BinaryReader br = new BinaryReader(stream))
            {
                int sequence_offset = br.ReadInt32();
                int start_index = br.ReadInt32();
                int num_stars = -br.ReadInt32(); // Remove the negative sign to get the correct number
                //Debug.Log($"Number of stars to load: {num_stars}");
                int star_number_settings = br.ReadInt32();
                int proper_motion_included = br.ReadInt32();
                int num_magnitudes = br.ReadInt32();
                int star_data_size = br.ReadInt32();

                int level = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

                for (int i = 0; i < num_stars; i++)
                {
                        float catalog_number = br.ReadSingle();
                        double right_ascension = br.ReadDouble();
                        double declination = br.ReadDouble();
                        char spectral_type = br.ReadChar();
                        byte spectral_index = br.ReadByte();
                        short magnitude = br.ReadInt16();
                        float ra_proper_motion = br.ReadSingle();
                        float dec_proper_motion = br.ReadSingle();

                        Star star = new Star(catalog_number, right_ascension, declination, spectral_type, spectral_index, magnitude, ra_proper_motion, dec_proper_motion);
                        stars.Add(star);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load star data: {e.Message}");
        }
        Debug.Log($"Number of stars loaded: {stars.Count}");
        return stars;
    }
}
