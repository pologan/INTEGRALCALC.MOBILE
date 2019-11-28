using INTEGRALCALCULATOR.MOBILE.Models;
using INTEGRALCALCULATOR.MOBILE.Models.Algoritms;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace INTEGRALCALC.MOBILE
{
    /// <summary>
    /// ViewModel Kalkulatora
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class CalcViewModel
    {
        // Komendy obsługi przycisków.
        public Command CmdSin { get; set; }
        public Command CmdCos { get; set; }
        public Command CmdTg { get; set; }
        public Command CmdPwr { get; set; }
        public Command CmdPlus { get; set; }
        public Command CmdMinus { get; set; }
        public Command CmdMult { get; set; }
        public Command CmdDiv { get; set; }
        public Command CmdOne { get; set; }
        public Command CmdTwo { get; set; }
        public Command CmdThree { get; set; }
        public Command CmdX { get; set; }
        public Command CmdFour { get; set; }
        public Command CmdFive { get; set; }
        public Command CmdSix { get; set; }
        public Command CmdE { get; set; }
        public Command CmdSeven { get; set; }
        public Command CmdEight { get; set; }
        public Command CmdNine { get; set; }
        public Command CmdDel { get; set; }
        public Command CmdPi { get; set; }
        public Command CmdZero { get; set; }
        public Command CmdDot { get; set; }
        public Command CmdEquals { get; set; }
        public Command CmdLBracket { get; set; }
        public Command CmdRBracket { get; set; }
        public Command CmdSqrt { get; set; }

        /// <summary>
        /// Komenda kliknięcia na początek przedziału.
        /// </summary>
        public Command CmdStart { get; set; }

        /// <summary>
        /// Komenda kliknięcia na koniec przedziału.
        /// </summary>
        public Command CmdEnd { get; set; }

        /// <summary>
        /// Komenda kliknięcia na wpisywani funkcji.
        /// </summary>
        public Command CmdFunc { get; set; }

        /// <summary>
        /// Komenda kliknięcia na kroki.
        /// </summary>
        public Command CmdSteps { get; set; }

        /// <summary>
        /// Czy aktywne jest wpisywanie funkcji?
        /// </summary>
        public bool IsFuncWritingActive { get; set; }

        /// <summary>
        /// Czy aktywne jest wpisywanie początku przedziału?
        /// </summary>
        public bool IsStartWritingActive { get; set; }

        /// <summary>
        /// Czy aktywne jest wpisywanie końca przedziału?
        /// </summary>
        public bool IsEndWritingActive { get; set; }

        /// <summary>
        /// Czy aktywne jest wpisywanie liczby kroków?
        /// </summary>
        public bool IsStepsWritingActive { get; set; }

        /// <summary>
        /// Tekst wpisywany w pole całkowanej funkcji.
        /// </summary>
        public string Func { get; set; } = "";

        /// <summary>
        /// Tekst wpisywany w pole począku przedziału
        /// </summary>
        public string A { get; set; } = "";

        /// <summary>
        /// Tekst wpisywany w pole końca przedziału.
        /// </summary>
        public string B { get; set; } = "";

        /// <summary>
        /// Tekst wpisywany w pole kroków
        /// </summary>
        public string Steps { get; set; } = "";

        /// <summary>
        /// Czy wyświetla się ekran ładowania?
        /// </summary>
        public bool IsLoading { get; set; } = false;

        /// <summary>
        /// Wynik metody prostokątów.
        /// </summary>
        public string Rect { get; private set; }

        /// <summary>
        /// Wynik metody Trapezów.
        /// </summary>
        public string Trap { get; private set; }

        /// <summary>
        /// Wynik metody Simpsona.
        /// </summary>
        public string Simp { get; private set; }

        /// <summary>
        /// Wynik metody Monte Carlo II.
        /// </summary>
        public string Mont { get; private set; }

        /// <summary>
        /// Konstruktor.
        /// </summary>
        public CalcViewModel()
        {
            CmdSin = new Command(() => SinClicked());
            CmdCos = new Command(() => CosClicked());
            CmdTg = new Command(() => TgClicked());
            CmdPwr = new Command(() => PwrClicked());
            CmdPlus = new Command(() => PlusClicked());
            CmdMinus = new Command(() => MinusClicked());
            CmdMult = new Command(() => MultClicked());
            CmdDiv = new Command(() => DivClicked());
            CmdOne = new Command(() => OneClicked());
            CmdTwo = new Command(() => TwoClicked());
            CmdThree = new Command(() => ThreeClicked());
            CmdX = new Command(() => XClicked());
            CmdFour = new Command(() => FourClicked());
            CmdFive = new Command(() => FiveClicked());
            CmdSix = new Command(() => SixClicked());
            CmdE = new Command(() => EClicked());
            CmdSeven = new Command(() => SevenClicked());
            CmdEight = new Command(() => EightClicked());
            CmdNine = new Command(() => NineClicked());
            CmdDel = new Command(() => DelClicked());
            CmdPi = new Command(() => PiClicked());
            CmdZero = new Command(() => ZeroClicked());
            CmdDot = new Command(() => DotClicked());
            CmdEquals = new Command(() => EqualsClicked());
            CmdLBracket = new Command(() => LBracketClicked());
            CmdRBracket = new Command(() => RBracketClicked());
            CmdSqrt = new Command(() => SqrtClicked());
            CmdFunc = new Command(() => FuncClicked());
            CmdStart = new Command(() => StartClicked());
            CmdEnd = new Command(() => EndClicked());
            CmdSteps = new Command(() => StepsClicked());

            FuncClicked();
        }

        private void FuncClicked()
        {
            IsFuncWritingActive = true;
            IsStartWritingActive = false;
            IsEndWritingActive = false;
            IsStepsWritingActive = false;
        }

        private void StartClicked()
        {
            IsFuncWritingActive = false;
            IsStartWritingActive = true;
            IsEndWritingActive = false;
            IsStepsWritingActive = false;
        }

        private void EndClicked()
        {
            IsFuncWritingActive = false;
            IsStartWritingActive = false;
            IsEndWritingActive = true;
            IsStepsWritingActive = false;
        }

        private void StepsClicked()
        {
            IsFuncWritingActive = false;
            IsStartWritingActive = false;
            IsEndWritingActive = false;
            IsStepsWritingActive = true;
        }

        private void SinClicked()
        {
            if (IsFuncWritingActive)
            {
                Func += "sin(";
            }
            else if (IsStartWritingActive)
            {
                
            }
            else if (IsEndWritingActive)
            {
                
            }
            else if (IsStepsWritingActive)
            {
                
            }
        }

        private void CosClicked()
        {
            if (IsFuncWritingActive)
            {
                Func += "cos(";
            }
            else if (IsStartWritingActive)
            {

            }
            else if (IsEndWritingActive)
            {

            }
            else if (IsStepsWritingActive)
            {

            }
        }

        private void TgClicked()
        {
            if (IsFuncWritingActive)
            {
                Func += "tg(";
            }
            else if (IsStartWritingActive)
            {

            }
            else if (IsEndWritingActive)
            {

            }
            else if (IsStepsWritingActive)
            {

            }
        }

        private void PwrClicked()
        {
            if (IsFuncWritingActive)
            {
                Func += "^";
            }
            else if (IsStartWritingActive)
            {
                A += "^";
            }
            else if (IsEndWritingActive)
            {
                B += "^";
            }
            else if (IsStepsWritingActive)
            {

            }
        }

        private void PlusClicked()
        {
            if (IsFuncWritingActive)
            {
                Func += "+";
            }
            else if (IsStartWritingActive)
            {
                A += "+";
            }
            else if (IsEndWritingActive)
            {
                B += "+";
            }
            else if (IsStepsWritingActive)
            {

            }
        }

        private void MinusClicked()
        {
            if (IsFuncWritingActive)
            {
                Func += "-";
            }
            else if (IsStartWritingActive)
            {
                A += "-";
            }
            else if (IsEndWritingActive)
            {
                B += "-";
            }
            else if (IsStepsWritingActive)
            {

            }
        }

        private void MultClicked()
        {
            if (IsFuncWritingActive)
            {
                Func += "*";
            }
            else if (IsStartWritingActive)
            {
                A += "*";
            }
            else if (IsEndWritingActive)
            {
                B += "*";
            }
            else if (IsStepsWritingActive)
            {

            }
        }

        private void DivClicked()
        {
            if (IsFuncWritingActive)
            {
                Func += "/";
            }
            else if (IsStartWritingActive)
            {

            }
            else if (IsEndWritingActive)
            {

            }
            else if (IsStepsWritingActive)
            {

            }
        }

        private void OneClicked()
        {
            if (IsFuncWritingActive)
            {
                Func += "1";
            }
            else if (IsStartWritingActive)
            {
                A += "1";
            }
            else if (IsEndWritingActive)
            {
                B += "1";
            }
            else if (IsStepsWritingActive)
            {
                Steps += "1";
            }
        }

        private void TwoClicked()
        {
            if (IsFuncWritingActive)
            {
                Func += "2";
            }
            else if (IsStartWritingActive)
            {
                A += "2";
            }
            else if (IsEndWritingActive)
            {
                B += "2";
            }
            else if (IsStepsWritingActive)
            {
                Steps += "2";
            }
        }

        private void ThreeClicked()
        {
            if (IsFuncWritingActive)
            {
                Func += "3";
            }
            else if (IsStartWritingActive)
            {
                A += "3";
            }
            else if (IsEndWritingActive)
            {
                B += "3";
            }
            else if (IsStepsWritingActive)
            {
                Steps += "3";
            }
        }

        private void XClicked()
        {
            if (IsFuncWritingActive)
            {
                Func += "x";
            }
            else if (IsStartWritingActive)
            {

            }
            else if (IsEndWritingActive)
            {

            }
            else if (IsStepsWritingActive)
            {

            }
        }

        private void FourClicked()
        {
            if (IsFuncWritingActive)
            {
                Func += "4";
            }
            else if (IsStartWritingActive)
            {
                A += "4";
            }
            else if (IsEndWritingActive)
            {
                B += "4";
            }
            else if (IsStepsWritingActive)
            {
                Steps += "4";
            }
        }

        private void FiveClicked()
        {
            if (IsFuncWritingActive)
            {
                Func += "5";
            }
            else if (IsStartWritingActive)
            {
                A += "5";
            }
            else if (IsEndWritingActive)
            {
                B += "5";
            }
            else if (IsStepsWritingActive)
            {
                Steps += "5";
            }
        }

        private void SixClicked()
        {
            if (IsFuncWritingActive)
            {
                Func += "6";
            }
            else if (IsStartWritingActive)
            {
                A += "6";
            }
            else if (IsEndWritingActive)
            {
                B += "6";
            }
            else if (IsStepsWritingActive)
            {
                Steps += "6";
            }
        }

        private void EClicked()
        {
            if (IsFuncWritingActive)
            {
                Func += "e";
            }
            else if (IsStartWritingActive)
            {
                A += "e";
            }
            else if (IsEndWritingActive)
            {
                B += "e";
            }
            else if (IsStepsWritingActive)
            {
                
            }
        }

        private void SevenClicked()
        {
            if (IsFuncWritingActive)
            {
                Func += "7";
            }
            else if (IsStartWritingActive)
            {
                A += "7";
            }
            else if (IsEndWritingActive)
            {
                B += "7";
            }
            else if (IsStepsWritingActive)
            {
                Steps += "7";
            }
        }

        private void EightClicked()
        {
            if (IsFuncWritingActive)
            {
                Func += "8";
            }
            else if (IsStartWritingActive)
            {
                A += "8";
            }
            else if (IsEndWritingActive)
            {
                B += "8";
            }
            else if (IsStepsWritingActive)
            {
                Steps += "8";
            }
        }

        private void NineClicked()
        {
            if (IsFuncWritingActive)
            {
                Func += "9";
            }
            else if (IsStartWritingActive)
            {
                A += "9";
            }
            else if (IsEndWritingActive)
            {
                B += "9";
            }
            else if (IsStepsWritingActive)
            {
                Steps += "9";
            }
        }

        private void DelClicked()
        {
            if (IsFuncWritingActive && Func.Length > 0)
            {
                Func = Func.Remove(Func.Length - 1);
            }
            else if (IsStartWritingActive && A.Length > 0)
            {
                A = A.Remove(A.Length - 1);
            }
            else if (IsEndWritingActive && B.Length > 0)
            {
                B = B.Remove(B.Length - 1);
            }
            else if (IsStepsWritingActive && Steps.Length > 0)
            {
                Steps = Steps.Remove(Steps.Length - 1);
            }
        }

        private void PiClicked()
        {
            if (IsFuncWritingActive)
            {
                Func += "pi";
            }
            else if (IsStartWritingActive)
            {
                A += "pi";
            }
            else if (IsEndWritingActive)
            {
                B += "pi";
            }
            else if (IsStepsWritingActive)
            {

            }
        }

        private void ZeroClicked()
        {
            if (IsFuncWritingActive)
            {
                Func += "0";
            }
            else if (IsStartWritingActive)
            {
                A += "0";
            }
            else if (IsEndWritingActive)
            {
                B += "0";
            }
            else if (IsStepsWritingActive)
            {
                if (Steps.Length > 0)
                {
                    Steps += "0";
                }
            }
        }

        private void DotClicked()
        {
            if (IsFuncWritingActive)
            {
                if (Func.Length == 0)
                {
                    Func += "0.";
                }
                else if (!Func.Contains("."))
                {
                    Func += ".";
                }
            }
            else if (IsStartWritingActive)
            {
                if (A.Length == 0)
                {
                    A += "0.";
                }
                else if (!A.Contains("."))
                {
                    A += ".";
                }
            }
            else if (IsEndWritingActive)
            {
                if (B.Length == 0)
                {
                    B += "0.";
                }
                else if (!B.Contains("."))
                {
                    B += ".";
                }
            }
            else if (IsStepsWritingActive)
            {

            }
        }

        private async void EqualsClicked()
        {
            double result;
            if (!string.IsNullOrEmpty(Func) && !string.IsNullOrEmpty(A) && !string.IsNullOrEmpty(B) && !string.IsNullOrEmpty(Steps))
            {
                IsLoading = true;
                result = await Task.Run(() => IntegrationAlgorithms.Rectanglular(A, B, int.Parse(Steps), Func));
                Rect = result.ToString().Replace(",", ".");
                result = await Task.Run(() => IntegrationAlgorithms.Trapezoidal(A, B, int.Parse(Steps), Func));
                Trap = result.ToString().Replace(",", ".");
                result = await Task.Run(() => IntegrationAlgorithms.Simpson(A, B, int.Parse(Steps), Func));
                Simp = result.ToString().Replace(",", ".");
                result = await Task.Run(() => IntegrationAlgorithms.MonteCarlo(A, B, int.Parse(Steps), Func));
                Mont = result.ToString().Replace(",", ".");
                IsLoading = false;
            }
        }

        private void LBracketClicked()
        {
            if (IsFuncWritingActive)
            {
                Func += "(";
            }
            else if (IsStartWritingActive)
            {

            }
            else if (IsEndWritingActive)
            {

            }
            else if (IsStepsWritingActive)
            {

            }
        }

        private void RBracketClicked()
        {
            if (IsFuncWritingActive)
            {
                Func += ")";
            }
            else if (IsStartWritingActive)
            {
                A += ")";
            }
            else if (IsEndWritingActive)
            {
                B += ")";
            }
            else if (IsStepsWritingActive)
            {

            }
        }

        private void SqrtClicked()
        {
            if (IsFuncWritingActive)
            {
                Func += "sqrt(";
            }
            else if (IsStartWritingActive)
            {
                A += "sqrt(";
            }
            else if (IsEndWritingActive)
            {
                B += "sqrt(";
            }
            else if (IsStepsWritingActive)
            {

            }
        }
    }
}
