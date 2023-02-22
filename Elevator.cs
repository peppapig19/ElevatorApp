using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace ElevatorApp
{
    class Elevator : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public int Cur { get; private set; } = 1;
        public readonly int count;
        enum DIRECTION { Up, Down, None };
        DIRECTION dir = DIRECTION.None;
        public enum STAND { Open, Closed };
        STAND doors = STAND.Open;
        public STAND Doors
        {
            get => doors;
            set
            {
                doors = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Doors"));
            }
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        public Elevator(int _count)
        {
            if (_count > 1) count = _count;
            else throw new Exception("Лифт не нужен");
        }

        public void Close() => Doors = STAND.Closed;

        public void Open()
        {
            if (dir != DIRECTION.None) return;

            Doors = STAND.Open;
        }

        public async Task MoveAsync(int n, IProgress<string> pr)
        {
            if (n < 1 || n > count) throw new Exception("Некорректный этаж");

            dir = Cur < n ? DIRECTION.Up : DIRECTION.Down;

            await Task.Run(() =>
            {
                while (Cur != n)
                {
                    Task.Delay(1000).Wait();

                    if (dir == DIRECTION.Up) Cur++;
                    if (dir == DIRECTION.Down) Cur--;

                    pr.Report(Cur.ToString());
                }
            });

            dir = DIRECTION.None;
            Open();
        }

        public async Task PickAsync(int n, int m, IProgress<string> pr)
        {
            if (Cur < m && m < n)
            {
                await MoveAsync(m, pr);
                await Task.Delay(3000);
                Close();
                await MoveAsync(n, pr);
            }
            else
            {
                await MoveAsync(n, pr);
                await Task.Delay(3000);
                Close();
                await MoveAsync(m, pr);
            }
        }
    }
}