using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class ChartCalc
    {
        public ChartCalc(double daimeter, double radialDepth, 
            int teethNumber, int stiffness, double damping, 
            double fn, string material){
            // Define parameters of milling
            d_fr = daimeter; // mm D of mill
            ae = radialDepth; // mm radial depth        
            Nt = teethNumber; // teeth number
            ke = stiffness; //~2400000
            zeta = damping; //~0.011
            wn = fn;
            Ks = material == "Titanium." ? 4000 : 700;
            beta = 68;
        }

        // Define parameters of milling
        double d_fr { set; get; } // mm D of mill
        double ae { set; get; } // mm radial depth        
        int Nt { set; get; } // mm D of mill

        // Define parameters of the system
        double ke { set; get; } // stiffnes, N/m
        double zeta { set; get; } // damping ratio
        double wn { set; get; } // frequency, (f) Hz    

        // Define specific force and force angle
        double Ks = 2000; // N/mm^2
        double beta = 68; // deg    


        public Dictionary<double, double> SLD(int lobeNumber)
        {
            // Define frequency, hz
            var w_step = 0.1;
            var w_count = (int)Math.Round((wn * 2) / w_step);
            List<double> w = new List<double>();
            var current_f = 0D;
            for (int i = 0; i < w_count; i++)
            {
                w.Add(current_f);
                current_f += w_step;
            }

            // radius of the mill
            double r_fr = d_fr / 2;
            // angels of start and exit
            double phis = 180 - ToDeg(Math.Acos((r_fr - ae) / r_fr));
            double phie = 180;

            // Define average number of teeth in cut, Nt_star
            var Nt_star = (phie - phis) * Nt / 360;

            // Define frequency ratio, rad/ s
            var ry = w.Select(freq => freq / wn).ToList();


            var FRF_real_y = ry.Select(r => (1 / ke * (1 - r * r) / (Math.Pow((1 - r * r), 2) + Math.Pow((2 * zeta * r), 2)))).ToList();
            var FRF_imag_y = ry.Select(r => (1 / ke * (-2 * zeta * r) / (Math.Pow((1 - r * r), 2) + Math.Pow((2 * zeta * r), 2)))).ToList();

            // Convert to mm/ N
            FRF_real_y = FRF_real_y.Select(frf_r => frf_r * 1000).ToList();
            FRF_imag_y = FRF_imag_y.Select(frf_im => frf_im * 1000).ToList();

            // Directional orientation factors

            //var muy = Math.Cos((beta - (180 - (phis + phie) / 2)) * Math.PI / 180) * Math.Cos((180 - (phis + phie) / 2) * Math.PI / 180);
            var muy = Math.Cos((beta - (180 - (phis + phie) / 2)) * Math.PI / 180) * Math.Cos((180 - (phis + phie) / 2) * Math.PI / 180);


            // Oriented FRF
            var FRF_real_orient = FRF_real_y.Select(frf_r => frf_r * muy).ToList();
            var FRF_imag_orient = FRF_imag_y.Select(frf_i => frf_i * muy).ToList();

            // Determine valid chatter frequency range
            for (int i = 0; i < FRF_real_orient.Count(); i++)
            {
                if (FRF_real_orient[i] < 0)
                {
                    var count = FRF_real_orient.Count() - i;
                    FRF_real_orient = FRF_real_orient.GetRange(i, count);
                    FRF_imag_orient = FRF_imag_orient.GetRange(i, count);
                    w = w.GetRange(i, count);
                    break;
                }
            }

            // Calculate blim

            var blim = FRF_real_orient.Select(frf_r => -1 / (2 * Ks * frf_r * Nt_star)).ToList();  // mm
            var epsilon = new List<double>();

            // Calculate epsilon
            for (int cnt = 0; cnt < FRF_real_orient.Count(); cnt++)
            {
                if (FRF_imag_orient[cnt] < 0)
                {
                    epsilon.Add(2 * Math.PI - 2 * Math.Atan(Math.Abs(FRF_real_orient[cnt] / FRF_imag_orient[cnt])));
                }
                else
                {
                    epsilon.Add(Math.PI - 2 * Math.Atan(Math.Abs(FRF_real_orient[cnt] / FRF_imag_orient[cnt])));
                }
            }

            // Calculate spindle speeds for N = 0 to 5
            var N0 = new List<double>();
            for (int i = 0; i < epsilon.Count(); i++)
            {
                N0.Add(w[i] * 60 / (Nt) / (lobeNumber + epsilon[i] / 2 / Math.PI));   // rpm
            }


            var dic = blim.Zip(N0, (k, v) => new { k, v })
             .ToDictionary(x => x.k, x => x.v);
            return dic;
        }

        public static double ToDeg(double radians)
        {
            double degrees = (180 / Math.PI) * radians;
            return (degrees);
        }

        private double ToRad(double angle)
        {
            return Math.PI * angle / 180.0;
        }


    }
}
