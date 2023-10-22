using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HomeWork2_1.ViewModels
{
    public class ThreedsVM : INotifyPropertyChanged
    {
        private int[] bubbleArray;
        private int[] insertionArray;
        
        private long t1_Time; 
        private long t2_Time;

        private Thread threed1;
        private Thread threed2;

        public event PropertyChangedEventHandler PropertyChanged;

        public long T1_Time
        {
            get { return t1_Time; }
            set
            {
                t1_Time = value;
                OnPropertyChanged();
            }
        }

        public long T2_Time
        {
            get { return t2_Time; }
            set
            {
                t2_Time = value;
                OnPropertyChanged();
            }
        }

        public int[] BubbleArray
        {
            get { return bubbleArray; }
            set
            {
                bubbleArray = value;
                OnPropertyChanged();
            }
        }

        public int[] InsertionArray
        {
            get { return insertionArray; }
            set
            {
                insertionArray = value;
                OnPropertyChanged();
            }
        }

        public void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public int[] ReadArray()
        {
                string[] lines = File.ReadAllLines("arr.txt");
                int[] result = new int[lines.Length];
                for (int i = 0; i < lines.Length; i++)
                {
                    if (int.TryParse(lines[i], out int number))
                        result[i] = number;
                    else
                        MessageBox.Show($"Помилка перетворення рядка {lines[i]} у ціле число.");
                }
                return result;
        }


        private void BubbleSort()
        {
            int l = bubbleArray.Length;
            bool swapped;
            do
            {
                swapped = false;
                for (int i = 1; i < l; i++)
                {
                    if (bubbleArray[i - 1] > bubbleArray[i])
                    {
                        int temp = bubbleArray[i - 1];
                        bubbleArray[i - 1] = bubbleArray[i];
                        bubbleArray[i] = temp;
                        swapped = true;
                    }
                }
            } while (swapped);
        }

        private void InsertionSort()
        {
            int l = insertionArray.Length;
            for (int i = 1; i < l; i++)
            {
                int key = insertionArray[i];
                int j = i - 1;
                while (j >= 0 && insertionArray[j] > key)
                {
                    insertionArray[j + 1] = insertionArray[j];
                    j--;
                }
                insertionArray[j + 1] = key;
            }
        }

        public ICommand Start
        {
            get
            {
                return new RelayCommand(() => { StartTreads(); });
            }
        }

        public void StartTreads()
        {
            bubbleArray = ReadArray();
            insertionArray = ReadArray();

            T1_Time = SortTime(BubbleSort);
            T2_Time = SortTime(InsertionSort);

            OnPropertyChanged(nameof(BubbleArray));
            OnPropertyChanged(nameof(InsertionArray));
            OnPropertyChanged(nameof(T1_Time));
            OnPropertyChanged(nameof(T2_Time));
        }

        private long SortTime(ThreadStart sortingMethod)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Thread thread = new Thread(sortingMethod);
            thread.Start();
            thread.Join();
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }


        public ICommand Stop
        {
            get
            {
                return new RelayCommand(() => { threed1.Abort(); threed2.Abort(); });
            }
        }
    }
}
