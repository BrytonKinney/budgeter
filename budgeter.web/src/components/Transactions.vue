<template>
  <div class="hello">
    <div class="lds-ring" v-show="isLoading">
      <div></div>
      <div></div>
      <div></div>
      <div></div>
    </div>
    <div>
      <div class="inline">
        <div class="inline">
          <label for="StartDate">Start Date</label>
          <datepicker name="StartDate" format="MM/dd/yyyy" v-model="requestHeaders.startDate"></datepicker>
        </div>
        <div class="inline">
          <label for="EndDate">End Date</label>
          <datepicker name="EndDate" format="MM/dd/yyyy" v-model="requestHeaders.endDate"></datepicker>
        </div>
      </div>
      <div class="inline">
        <a
          class="table-page"
          v-bind:key="pageNum"
          v-for="pageNum in pages"
          v-on:click.prevent="pageChanged(pageNum)"
        >{{ pageNum }}</a>
      </div>
      <div class="inline">
        <select v-model="requestHeaders.take">
          <option value="10">10</option>
          <option value="20">20</option>
          <option value="30">30</option>
          <option v-bind:value="maxPageLength">{{ maxPageLength }}</option>
        </select>
      </div>
      <div class="inline">
        <input
          id="IgnorePayments"
          name="IgnorePayments"
          type="checkbox"
          v-model="requestHeaders.ignorePayments"
        />
        <label for="IgnorePayments">Ignore Payments?</label>
      </div>
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
import Datepicker from "../../node_modules/vuejs-datepicker";
export default {
  name: "Transactions",
  components: {
    Datepicker
  },
  data: function() {
    return {
      isLoading: true,
      requestHeaders: {
        skip: 0,
        take: 10,
        ignorePayments: false,
        startDate: null,
        endDate: null
      },
      maxPageLength: "All",
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
    pageNum: function(value) {
      this.requestHeaders.skip = pageNum - 1;
    },
    "requestHeaders.take": function(value) {
      this.getTransactions();
    },
    "requestHeaders.ignorePayments": function() {
      this.getTransactions();
    },
    "requestHeaders.startDate": function() {
      this.getTransactions();
    },
    "requestHeaders.endDate": function() {
      this.getTransactions();
    },
    "trxData.total": function(value, oldValue) {
      if (this.requestHeaders.take === oldValue)
        this.requestHeaders.take = value;
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
    encodeValue: function(value) {
      if (typeof value.getMonth === "function") {
        console.log("Found datetime");
        return encodeURIComponent(value.toLocaleDateString());
      }
      return encodeURIComponent(value);
    },
    encodeUriParams: function(params) {
      return Object.keys(params)
        .filter(k => {
          console.log(`${k}: ${params[k]}`);
          return params[k] !== null;
        })
        .map(
          key => `${encodeURIComponent(key)}=${this.encodeValue(params[key])}`
        )
        .join("&");
    },
    pageChanged: function(pageNum) {
      if (this.requestHeaders.take === this.maxPageLength) pageNum = 1;
      this.requestHeaders.skip = (pageNum - 1) * this.requestHeaders.take;
      this.getTransactions();
    },
    getTransactions: function() {
      this.isLoading = true;
      if (this.requestHeaders.take === this.maxPageLength)
        this.requestHeaders.skip = 0;
      fetch(
        `https://localhost:44341/Transactions?${this.encodeUriParams(
          this.requestHeaders
        )}`,
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
          if (this.pages.length > 0) this.pages = [];
          for (
            var i = 1;
            i <= Math.floor(data.total / this.requestHeaders.take);
            i++
          )
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
.inline {
  display: inline-flex;
  padding-right: 0.25em;
  label {
    padding-right: 0.25em;
  }
}
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
