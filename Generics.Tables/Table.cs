using System;
using System.Collections.Generic;

namespace Generics.Tables {
    class Table<TRow, TCol, TVal> {
        private TableData<TRow, TCol, TVal> data = new TableData<TRow, TCol, TVal>();
        public TableOpen<TRow, TCol, TVal> Open { get; private set; }
        public TableExisted<TRow, TCol, TVal> Existed { get; private set; }

        public Dictionary<TRow, bool> Rows {
            get {
                return data.rows;
            }
        }

        public Dictionary<TCol, bool> Columns {
            get {
                return data.cols;
            }
        }

        public Table() {
            Open = new TableOpen<TRow, TCol, TVal>(data);
            Existed = new TableExisted<TRow, TCol, TVal>(data);
        }

        public void AddRow(TRow row) {
            data.AddRow(row);
        }

        public void AddColumn(TCol col) {
            data.AddColumn(col);
        }
    }

    class TableData<TRow, TCol, TVal> {
        private readonly Dictionary<TRow, Dictionary<TCol, TVal>> dict = new Dictionary<TRow, Dictionary<TCol, TVal>>();
        public readonly Dictionary<TRow, bool> rows = new Dictionary<TRow, bool>();
        public readonly Dictionary<TCol, bool> cols = new Dictionary<TCol, bool>();

        public void AddRow(TRow row) {
            rows[row] = true;
        }

        public void AddColumn(TCol col) {
            cols[col] = true;
        }

        public TVal this[TRow row, TCol col] {
            get {
                return dict[row][col];
            }
            set {
                AddRow(row);
                AddColumn(col);

                if (!dict.ContainsKey(row)) {
                    dict[row] = new Dictionary<TCol, TVal>();
                }
                dict[row][col] = value;
            }
        }

        public bool DoesContain(TRow row, TCol col) {
            return dict.ContainsKey(row) && dict[row].ContainsKey(col);
        }

        public bool DoKeysExist(TRow row, TCol col) {
            return rows.ContainsKey(row) && cols.ContainsKey(col);
        }
    }

    class TableOpen<TRow, TCol, TVal> {
        private TableData<TRow, TCol, TVal> data;
        public TableOpen(TableData<TRow, TCol, TVal> data) {
            this.data = data;
        }

        public TVal this[TRow row, TCol col] {
            get {
                if (data.DoesContain(row, col)) {
                    return data[row, col];
                }
                return default(TVal);
            }
            set {
                data[row, col] = value;
            }
        }
    }

    class TableExisted<T1, T2, T3> {
        private TableData<T1, T2, T3> data;
        public TableExisted(TableData<T1, T2, T3> data) {
            this.data = data;
        }

        public T3 this[T1 row, T2 col] {
            get {
                if (data.DoesContain(row, col)) {
                    return data[row, col];
                }
                if (data.DoKeysExist(row, col)) {
                    return default(T3);
                }
                throw new ArgumentException();
            }
            set {
                if (data.DoKeysExist(row, col)) {
                    data[row, col] = value;
                }
                else {
                    throw new ArgumentException();
                }
            }
        }
    }
}
