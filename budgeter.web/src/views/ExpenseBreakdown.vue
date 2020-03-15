<template>
  <div class="expenses">
    <ExpensesChart />
  </div>
</template>

<script>
// @ is an alias to /src
import ExpensesChart from '@/components/ExpensesChart.vue'

export default {
  name: 'ExpenseBreakdown',
  components: {
    ExpensesChart
  }
}
</script>
