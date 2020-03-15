<template>
	<form method="POST">
		<input type="file" v-on:change="bindFileData($event)" name="transactions" />
		<button type="submit" v-on:click.prevent="uploadFile">Upload</button>
	</form>
</template>
<script>
export default {
  name: "FileUpload",
  data: function() {
	  return {
		  fileToUpload: {}
	  };
  },
  methods: {
	  bindFileData: function(e) {
		  this.fileToUpload = e.target.files[0];
	  },
	  uploadFile: function() {
		  if(this.fileToUpload) {
			  var fileForm = new FormData();
			  fileForm.append('transactions', this.fileToUpload);
			  fetch('https://localhost:44341/Transactions/upload', {
				  method: 'POST',
				  mode: 'cors',
				  body: fileForm
			  });
		  }
	  }
  }
}
</script>