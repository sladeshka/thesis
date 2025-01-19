<template>
  <footer class="footer bg-light text-center text-lg-start mt-auto py-4">
    <div class="container">
      <div class="row">
        <div class="col-md-6 footer-column">
          <h4>Company</h4>
          <a href="#">link 1</a><br/>
          <a href="#">link 2</a>
        </div>
        <div class="col-md-6 footer-column">
          <h4>Contacts</h4>
          <p>mail@mail.mail</p>
          <p>1234567890</p>
        </div>
      </div>
    </div>
  </footer>
</template>

<script lang="ts">
  export default {
    name: 'Footer',
  };
</script>

<style scoped>
  .footer {
    background-color: #f1e6d6;
    box-shadow: 0 2px 5px rgba(241, 230, 214, 0.9);
  }

  .footer-column h4 {
    color: #d69a6b;
  }
</style>
