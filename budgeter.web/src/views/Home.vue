<template>
  <div class="home">
    <Transactions />
  </div>
</template>

<script>
// @ is an alias to /src
import Transactions from '@/components/Transactions.vue'

export default {
  name: 'Home',
  components: {
    Transactions
  }
}
</script>
