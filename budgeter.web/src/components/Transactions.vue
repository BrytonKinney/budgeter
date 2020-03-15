<template>
  <div class="hello">
    <div class="lds-ring" v-show="isLoading">
      <div></div>
      <div></div>
      <div></div>
      <div></div>
    </div>
    <div>
      <a
        class="table-page"
        v-bind:key="pageNum"
        v-for="pageNum in pages"
        v-on:click.prevent="pageChanged(pageNum)"
      >{{ pageNum }}</a>
      <select v-model="take">
        <option value="10">10</option>
        <option value="20">20</option>
        <option value="30">30</option>
        <option v-bind:value="maxPageLength">{{ maxPageLength }}</option>
      </select>
      <input type="checkbox" v-model="ignorePayments" /> Ignore Payments?
    </div>
    <table>
      <thead>
        <tr>
          <th
            v-bind:key="itemIndex"
            v-for="(item, itemIndex) in fields"
            v-html="formatHeader(item)"
          ></th>
        </tr>
      </thead>
      <tbody v-if="trxData.data.length">
        <tr v-bind:key="trxIndex" v-for="(trx, trxIndex) in trxData.data">
          <td
            v-bind:key="field.binding"
            v-for="field in fields"
            v-bind:class="field.conditionalFormat ? field.conditionalFormat(trx[field.binding]) : ''"
          >{{ field.format ? field.format(trx[field.binding]) : trx[field.binding] }}</td>
        </tr>
      </tbody>
      <tbody v-else>
        <tr>
          <td v-bind:colspan="fields.length">No data available.</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
export default {
  name: "Transactions",

  data: function() {
    return {
      isLoading: true,
      skip: 0,
      take: 10,
      maxPageLength: 'All',
      ignorePayments: false,
      fields: [
        {
          display: "Transaction Date",
          binding: "transactionDate",
          format: function(data) {
            return new Date(data).toLocaleDateString();
          }
        },
        {
          display: "Post Date",
          binding: "postDate",
          format: function(data) {
            return new Date(data).toLocaleDateString();
          }
        },
        { display: "Reference ID", binding: "referenceId" },
        { display: "Description", binding: "description" },
        {
          display: "Amount",
          binding: "amount",
          conditionalFormat: function(data) {
            if (data < 0) return "negative-money";
            return "";
          },
          format: function(data) {
            var fixedDec = Math.abs(data).toFixed(2);
            var moneyStr = data < 0 ? `($${fixedDec})` : `$${fixedDec}`;
            return `${moneyStr}`;
          }
        },
        { display: "Account Number", binding: "accountNumber" },
        { display: "Card Number", binding: "cardNumber" },
        { display: "Cardholder Name", binding: "cardholderName" },
        { display: "MCC", binding: "mcc" },
        { display: "MCC Description", binding: "mccDescription" },
        { display: "MCC Group", binding: "mccGroup" }
      ],
      trxData: { data: [] },
      pages: []
    };
  },

  watch: {
    take: function(value) {
      this.getTransactions();
    },
    ignorePayments: function() {
      this.getTransactions();
    },
    'trxData.total': function(value) {
      this.take = value;
      this.maxPageLength = value;
    }
  },

  methods: {
    formatHeader: function(item) {
      if (item.display === "Amount") {
        var sum = 0;
        for (var idx in this.trxData.data) {
          if (this.trxData.data.hasOwnProperty(idx)) {
            sum += this.trxData.data[idx].amount;
          }
        }
        return `<div>${item.display}<br/>${sum.toFixed(2)}</div>`;
      }
      return item.display;
    },
    encodeUriParams: function(params) {
      return Object.keys(params)
        .map(
          key => `${encodeURIComponent(key)}=${encodeURIComponent(params[key])}`
        )
        .join("&");
    },
    pageChanged: function(pageNum) {
      console.log(pageNum);
      this.skip = pageNum * this.take;
      this.getTransactions();
    },
    getTransactions: function() {
      this.isLoading = true;
      fetch(
        `https://localhost:44341/Transactions?${this.encodeUriParams({
          skip: this.skip,
          take: this.take,
          ignorePayments: this.ignorePayments
        })}`,
        {
          headers: {
            "Content-Type": "application/json"
          }
        }
      )
        .then(resp => {
          return resp.json();
        })
        .then(data => {
          this.trxData = data;
          console.log(data);
          if (this.pages.length > 0) {
            console.log(`Updating page dropdown: ${this.pages}`);
            this.pages = [];
          }
          for (var i = 1; i <= Math.floor(data.total / this.take); i++)
            this.pages.push(i);
          this.isLoading = false;
        });
    }
  },

  created: function() {
    this.getTransactions();
  }
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
.table-page {
  padding: 0.5em;
  cursor: pointer;
}
table,
td,
th {
  border-collapse: collapse;
  border: 1px solid rgb(241, 238, 218);
}
th {
  background-color: rgb(115, 172, 92);
  color: rgb(0, 0, 0);
}
td {
  background-color: rgb(0, 0, 0);
  color: rgb(115, 172, 92);
  padding: 0.25em;
}
.negative-money {
  color: red;
}
h3 {
  margin: 40px 0 0;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
.lds-ring {
  display: inline-block;
  position: absolute;
  top: 50%;
  width: 80px;
  height: 80px;
}
.lds-ring div {
  box-sizing: border-box;
  display: block;
  position: absolute;
  width: 64px;
  height: 64px;
  border: 8px solid #fff;
  border-radius: 50%;
  animation: lds-ring 1.2s cubic-bezier(0.5, 0, 0.5, 1) infinite;
  border-color: #fff transparent transparent transparent;
}
.lds-ring div:nth-child(1) {
  animation-delay: -0.45s;
}
.lds-ring div:nth-child(2) {
  animation-delay: -0.3s;
}
.lds-ring div:nth-child(3) {
  animation-delay: -0.15s;
}
@keyframes lds-ring {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}
</style>
