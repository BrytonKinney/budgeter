<template>
  <div>
    <div>
      <input
        type="checkbox"
        v-model="groupByOptions.mccGroup"
        v-on:change="updateDataSetForGroupOptions"
      />MCC Group
      <input
        type="checkbox"
        v-model="groupByOptions.mccDescription"
        v-on:change="updateDataSetForGroupOptions"
      />MCC Description
    </div>
    <div class="expense-chart">
      <canvas id="pieChart" width="250" height="250"></canvas>
    </div>
  </div>
</template>
<script>
import Chart from "../../node_modules/chart.js/dist/Chart.js";
export default {
  name: "ExpensesChart",
  data: function() {
    return {
      expenses: [],
      pieChart: null,
      groupByOptions: { mccGroup: false, mccDescription: false },
      expenseChartData: {
        datasets: [{ data: [], backgroundColor: [] }],
        labels: []
      },
      colors: []
    };
  },
  methods: {
    updateDataSetForGroupOptions() {
	  this.expenseChartData.datasets.data = [];
	  this.expenseChartData.datasets.backgroundColor = [];
	  this.expenseChartData.labels = [];
      var selectedOpts = [];
      for (var opt in this.groupByOptions) {
        if (this.groupByOptions.hasOwnProperty(opt)) {
          if (this.groupByOptions[opt]) selectedOpts.push(opt);
        }
      }
      if (selectedOpts.length === 1) {
        this.setDataset(this.groupBy(this.expenses, selectedOpts[0]));
      } else if (selectedOpts.length === 2) {
        this.setDataset(this.groupByMultiple(this.expenses, function(item) { return [item.mccGroup, item.mccDescription] }));
      }
    },
    // modified from https://stackoverflow.com/a/54024888
    groupByMultiple: function(array, f) {
      let groups = {};
      array.forEach(function(o) {
        var group = JSON.stringify(f(o));
        groups[f(o).join(" - ")] = groups[f(o).join(" - ")] || [];
        groups[f(o).join(" - ")].push(o);
      });
      return groups;
    },
    groupBy: function(arr, key) {
      return arr.reduce(function(rv, x) {
        (rv[x[key]] = rv[x[key]] || []).push(x);
        return rv;
      }, {});
    },
    getRandomColor: function(max, min) {
      return Math.floor(Math.random() * (max - min + 1) + min);
    },
    setDataset: function(grpData) {
      this.expenseChartData.datasets[0].data = [];
      for (var group in grpData) {
        if (grpData.hasOwnProperty(group)) {
          this.expenseChartData.labels.push(group);
          this.colors.push(
            `rgba(${this.getRandomColor(255, 0).toFixed(
              2
            )}, ${this.getRandomColor(255, 0).toFixed(
              2
            )}, ${this.getRandomColor(255, 0).toFixed(2)}, 1)`
          );
          var sum = 0.0;
          for (var dataPoint in grpData[group]) {
            sum += Math.abs(grpData[group][dataPoint].amount);
          }
          this.expenseChartData.datasets[0].data.push(sum.toFixed(2));
        }
      }
      this.expenseChartData.datasets[0].backgroundColor = this.colors;
      this.pieChart.update();
    }
  },
  created: function() {
    fetch("https://localhost:44341/Transactions/expenses")
      .then(resp => resp.json())
      .then(data => {
        this.expenses = data;
        this.pieChart = new Chart(document.getElementById("pieChart"), {
          type: "pie",
          data: this.expenseChartData
        });
      });
  }
};
</script>
<style lang="scss">
.expense-chart {
  position: relative;
  height: 50vh;
  width: 50vw;
  margin-left: auto;
  margin-right: auto;
}
</style>