using System;
using UnityEngine;

namespace REG
{
    public class RegLogic
    {
        private bool Mozno;
        public bool MOZNO;
        
        // ======== монстрофункция ==========================================
        public bool CalculateLogic (String posName, int point) //out bool Mozno
        {

            switch (posName)
            {
                // когда вытянул руку - можно ходить только за его спиной! - в памятку
                
                // палка вверх
                case "1A" or "1B" or "1C" or "1D":
                    Mozno = false;
                    break;
                
                // боком А
                case "2A" or "2C" or "3A" or "3C":
                    switch (point)
                    {
                        case 0 or 1 or 4 or 5:
                            Mozno = true;
                            break;
                        case 2 or 3 or 6 or 7:
                            Mozno = false;
                            break;
                        default:
                            Debug.Log("Передали невалидный пойнт");
                            break;
                    }
                    break;
                
                case "2B" or "2D" or "3B" or "3D":
                    switch (point)
                    {
                        case 0 or 1 or 4 or 5:
                            Mozno = false;
                            break;
                        case 2 or 3 or 6 or 7:
                            Mozno = true;
                            break;
                        default:
                            Debug.Log("Передали невалидный пойнт");
                            break;
                    }
                    break;

                
                case "4A":
                    switch (point)
                    { 
                        case 4 or 5:
                            Mozno = true;
                            break;
                        case 0 or 1 or 2 or 3 or 6 or 7:
                            Mozno = false;
                            break;
                        default:
                            Debug.Log("Передали невалидный пойнт");
                            break;
                    }
                    break;

                
                case "4B":
                    switch (point)
                    {    
                        case 6 or 7: 
                            Mozno = true; 
                            break;
                        case 7 or 2 or 3 or 4 or 5 or 0:
                            Mozno = false;
                            break;
                        default:
                            Debug.Log("Передали невалидный пойнт");
                            break;
                    }
                    break;
                   
                
                case "4C":
                    switch (point)
                    {    
                        case 0 or 1: 
                            Mozno = true; 
                            break;
                        case 2 or 3 or 4 or 5 or 6 or 7:
                            Mozno = false;
                            break;
                        default:
                            Debug.Log("Передали невалидный пойнт");
                            break;
                    }
                    break;
                
                case "4D":
                    switch (point)
                    {    
                        case 2 or 3: 
                            Mozno = true; 
                            break;
                        case 0 or 1 or 4 or 5 or 6 or 7:
                            Mozno = false;
                            break;
                        default:
                            Debug.Log("Передали невалидный пойнт");
                            break;
                    }
                    break;
                
                
                default:
                    Debug.Log(" Передали невалидную позицию");
                    break;
            }// конец свича

            MOZNO = Mozno;
            return MOZNO; // это у меня тут типо инкапсуляция физ знает зачем

        } // конец метода
    }
}